using GameProtocol.constans;
using GameProtocol;
using GameProtocol.dto.fight;
using SpaceNetServer.tool;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProtocol.dto;

namespace SpaceNetServer.logic.fight
{
    class FightRoom:AbsMulitHandler, HandlerInterface
    {
        public Dictionary<int, AbsFightModel> team = new Dictionary<int, AbsFightModel>();

        private List<int> off = new List<int>();

        private List<int> enterList = new List<int>();

        private int heroCount;
        public void init(SelectModel[] team) {
            heroCount = team.Length;
            
            this.team.Clear();
            off.Clear();
            //初始化英雄数据
            foreach (var item in team)
            {
               this.team.Add(item.userId, create(item,1));
            }
    
            enterList.Clear();
        }

        
        private FightPlayerModel create(SelectModel model,int team) {
            FightPlayerModel player = new FightPlayerModel();
            player.id = model.userId;
            player.code = model.hero;
            player.type = ModelType.HUMAN;
            player.name = getUser(model.userId).name;
            player.exp = 0;
            player.level = 1;
            player.free = 1;
            player.money = 0;
            player.team = team;
            //从配置表里 去出对应的英雄数据
           HeroDataModel data = HeroData.heroMap[model.hero];
           player.hp = data.hpBase;
           player.maxHp = data.hpBase;
           player.atk = data.atkBase;
           player.def = data.defBase;
           player.aSpeed = data.aSpeed;
           player.speed = data.speed;
           player.aRange = data.range;
           player.eyeRange = data.eyeRange;
           player.skills = initSkill(data.skills);
           player.equs = new int[3];
            return player;
        }

        private FightSkill[] initSkill(int[] value) {
            FightSkill[] skills=new FightSkill[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                int skillCode = value[i];
                SkillDataModel data = SkillData.skillMap[skillCode];
                SkillLevelData levelData = data.levels[0];
                FightSkill skill = new FightSkill(skillCode, 0, levelData.level, levelData.time, data.name, levelData.range, data.info, data.target, data.type);
                skills[i] = skill;
            }
            return skills;
        }

        public void ClientClose(NetFrame.UserToken token, string error)
        {
            leave(token);
            int userId = getUserId(token);
            if (team.ContainsKey(userId)) {
                if (!off.Contains(userId)) {
                    off.Add(userId);
                }
            }
            if (off.Count == heroCount) {
                EventUtil.destoryFight(GetArea());
                }

        }

        public void MessageReceive(NetFrame.UserToken token, NetFrame.auto.SocketModel message)
        {
            switch (message.command) { 
                case FightProtocol.ENTER_CREQ:
                    enterBattle(token);
                    break;
                case FightProtocol.MOVE_CREQ:
                    move(token, message.GetMessage<MoveDTO>());
                    break;
                case FightProtocol.ATTACK_CREQ:
                    attack(token, message.GetMessage<int>());
                    break;
                case FightProtocol.DAMAGE_CREQ:
                    damage(token, message.GetMessage<DamageDTO>());
                    break;
                case FightProtocol.SKILL_UP_CREQ:
                    skillUp(token, message.GetMessage<int>());
                    break;
                case FightProtocol.SKILL_CREQ:
                    skill(token, message.GetMessage<SkillAtkModel>());
                    break;

            }
        }

        private void skill(UserToken token, SkillAtkModel value)
        {
            value.userId = getUserId(token);
            brocast(FightProtocol.SKILL_BRO, value);
        }

        private void skillUp(UserToken token,int value)
        {
            int userId = getUserId(token);
            FightPlayerModel player;
            player = (FightPlayerModel)team[userId];
          
            if (player.free > 0) {
                //遍历角色技能列表 判断是否有此技能
                foreach (FightSkill item in player.skills)
                {
                    if (item.code == value) {
                        //判断玩家等级 是否达到技能提升等级
                        if (item.nextLevel != -1 && item.nextLevel <= player.level) {
                            player.free -= 1;
                            int level = item.level + 1;
                            SkillLevelData data= SkillData.skillMap[value].levels[level];
                            item.nextLevel = data.level;
                            item.range = data.range;
                            item.time = data.time;
                            item.level = level;
                            write(token, FightProtocol.SKILL_UP_SRES, item);
                        }
                        return;
                    }
                }
            }
        }

        private void damage(UserToken token,DamageDTO value)
        {
            int userId=getUserId(token);
            AbsFightModel atkModel;
            int skillLevel = 0;
            //判断攻击者是玩家英雄 还是小兵
            if (value.userId >= 0)
            {
                if (value.userId != userId) return;
                atkModel = getPlayer(userId);
                if (value.skill > 0) {
                    skillLevel = (atkModel as FightPlayerModel).SkillLevel(value.skill);
                    if (skillLevel == -1) {
                        return;
                    }
                }
            }
            else {
                //TODO:
                atkModel = getPlayer(userId);
            }
            //获取技能算法
            //循环获取目标数据 和攻击者数据 进行伤害计算 得出伤害数值
            if (!SkillProcessMap.has(value.skill)) return;
            ISkill skill = SkillProcessMap.get(value.skill);
            List<int[]> damages = new List<int[]>();
            foreach (int[] item in value.target)
            {
                AbsFightModel target = getPlayer(item[0]);
                skill.damage(skillLevel,ref atkModel,ref target,ref damages);
                if (target.hp == 0) {
                    switch (target.type) { 
                        case ModelType.HUMAN:
                            if (target.id > 0)
                            {
                                //击杀英雄
                                //启动定时任务 指定时间之后发送英雄复活信息 并且将英雄数据设置为满状态
                            }
                            else { 
                                //击杀小兵
                                //移除小兵数据
                            }
                            break;
                        case ModelType.BUILD:
                            //打破了建筑 给钱
                            
                            break;
                    }
                }
            }
            value.target = damages.ToArray();
            brocast(FightProtocol.DAMAGE_BRO, value);
        }

        AbsFightModel getPlayer(int userId)
        {
               return team[userId];
        }

        private void attack(UserToken token, int value)
        {
            AttackDTO atk = new AttackDTO();
            atk.userId = getUserId(token);
            atk.targetId = value;
            brocast(FightProtocol.ATTACK_BRO,atk);
        }

        private void move(UserToken token,MoveDTO value)
        { 
            int userId=getUserId(token);
            value.userId=userId;
            brocast(FightProtocol.MOVE_BRO, value);
        }

        private  void enterBattle(UserToken token)
        {

            int userId = getUserId(token);
                if (isEntered(token)) return;
                base.enter(token);
                if (!enterList.Contains(userId)) {
                    enterList.Add(userId);
                }
                //所有人准备了 发送房间信息
                if (enterList.Count == heroCount)
                {
                    FightRoomModel room = new FightRoomModel();
                    room.teamOne = team.Values.ToArray();
                    brocast(FightProtocol.START_BRO, room);
                }
            
        }

        public override byte GetType()
        {
            return Protocol.TYPE_FIGHT;
        }
    }
}
