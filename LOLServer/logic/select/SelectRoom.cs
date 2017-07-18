using GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using GameProtocol.dto;
using SpaceNetServer.tool;
using NetFrame;
using SpaceNetServer.dao.model;

namespace SpaceNetServer.logic.select
{
   public class SelectRoom:AbsMulitHandler, HandlerInterface
    {
       public ConcurrentDictionary<int, SelectModel> team = new ConcurrentDictionary<int, SelectModel>();
       
       //当前进入房间的人数
       int enterCount = 0;
       //当前定时任务ID
       int missionId = -1;

      public List<int> readList = new List<int>();

       public void Init(List<int> team)
       {
           //初始化房间数据
           this.team.Clear();
           enterCount = 0;
           foreach (int item in team)
           {
               SelectModel select = new SelectModel();
               select.userId = item;
               select.name = getUser(item).name;
               select.hero = -1;
               select.enter = false;
               select.ready = false;
               this.team.TryAdd(item, select);
           }
         
        
           //初始化完毕  开始定时任务  设定 30秒后没有进入选择界面的时候 直接解散此次匹配
          missionId= ScheduleUtil.Instance.schedule(delegate { 
            //30秒后判断进入情况 如果不是全员进入 解散房间
              if (enterCount < team.Count)
              {
                  destory();
              }
              else {
                  //再次启动定时任务 30秒内完成选人
                  missionId = ScheduleUtil.Instance.schedule(delegate {
                      //时间抵达30s 遍历判断 是否所有人都选择了英雄
                      bool selectAll = true;
                      foreach (SelectModel item in this.team.Values)
                      {
                          if (item.hero == -1) {
                              selectAll = false;
                              break;
                          }
                      }
                     

                      if (selectAll)
                      {
                          //全部选了 只是有人没有点准备按钮  开始战斗
                          StartFight();
                      }
                      else {
                          //有人没选   解散房间
                          destory();
                      }

                  }, 20 * 1000);
              }
           
           }, 20 * 1000);
       }
       /// <summary>
       /// 解散房间
       /// </summary>
       private void destory() {           
           //通知房间所有人 房间解散了 回主界面去
           brocast(SelectProtocol.DESTORY_BRO, null);
           //通知管理器 移除自身
           EventUtil.destorySelect(GetArea());
           //当前有定时任务 则进行关闭
           if (missionId != -1) {
               ScheduleUtil.Instance.removeMission(missionId);
           }
           
       }

       public void ClientClose(NetFrame.UserToken token, string error)
       {
           //调用离开方法 让此连接不再接收网络消息
           leave(token);
           //通知房间其他人 房间解散了 回主界面去
           brocast(SelectProtocol.DESTORY_BRO, null);
           //通知管理器 移除自身
           EventUtil.destorySelect(GetArea());
       }

       public void MessageReceive(NetFrame.UserToken token, NetFrame.auto.SocketModel message)
       {
           switch (message.command) { 
               case SelectProtocol.ENTER_CREQ:
                   enter(token);
                   break;
               case SelectProtocol.SELECT_CREQ:
                   select(token, message.GetMessage<int>());
                   break;
               case SelectProtocol.TALK_CREQ:
                   talk(token, message.GetMessage<string>());
                   break;
               case SelectProtocol.READY_CREQ:
                   ready(token);
                   break;
           }
       }

       private void ready(UserToken token)
       {
           //判断玩家是否在房间里
           if (!base.isEntered(token)) { return; }
           int userId = getUserId(token);
           //判断玩家是否已准备
           if (readList.Contains(userId)) return;
           SelectModel sm = null;
           //获取玩家选择数据模型
           if (team.ContainsKey(userId))
           {
               sm = team[userId];
           }
       
           //没选择英雄 不让准备
           if (sm.hero == -1)
           {

           }
           else {
               //设为已选状态 并通知其他人
               sm.ready = true;
               brocast(SelectProtocol.READY_BRO, sm);
               //添加进准备列表
               readList.Add(userId);
               if (readList.Count >= team.Count) { 
                //所有人都准备了 开始战斗
                   StartFight();
               }
           }
       }

       private void StartFight() {
           if (missionId != -1) {
               ScheduleUtil.Instance.removeMission(missionId);
               missionId = -1;
           }
           //通知战斗模块 创建战斗房间
           EventUtil.createFight(team.Values.ToArray());
           brocast(SelectProtocol.FIGHT_BRO, null);
           //通知选择房间管理器 销毁当前房间
           EventUtil.destorySelect(GetArea());
       }

       private void talk(UserToken token, string value) {
           //判断玩家是否在房间里
           if (!base.isEntered(token)) { return; }
           USER user = getUser(token);
           brocast(SelectProtocol.TALK_BRO, user.name + ":" + value);


           ///队伍聊天模式
       /*    if (teamOne.ContainsKey(user.id))
           {
               writeToUsers(teamOne.Keys.ToArray(), GetType(), GetArea(), SelectProtocol.TALK_BRO, user.name + ":" + value);
           }
           else {
               writeToUsers(teamTwo.Keys.ToArray(), GetType(), GetArea(), SelectProtocol.TALK_BRO, user.name + ":" + value);
           }*/
       }

       private void select(UserToken token,int value) {
           //判断玩家是否在房间里
           if (!base.isEntered(token)) { return; }
           USER user = getUser(token);
           //判断玩家是否拥有此英雄
           if (!user.heroList.Contains(value)) {
               write(token, SelectProtocol.SELECT_SRES, null);
               return;
           }
           //判断英雄队友是否已选
           SelectModel selectModel = null;
           if (team.ContainsKey(user.id))
           {
               foreach (SelectModel item in team.Values)
               {
                   if (item.hero == value) return;
               }
               selectModel = team[user.id];
           }
        
           //选择成功 通知房间所有人变更数据
           selectModel.hero = value;
           brocast(SelectProtocol.SELECT_BRO, selectModel);
       }

       private new void enter(UserToken token)
       {
           //判断用户所在的房间 并对其进入状态进行修改
           int userId = getUserId(token);
           if (team.ContainsKey(userId))
           {
               team[userId].enter = true;
           }
           else
           {
               return;
           }
           //判断用户是否已经在房间 不在则计算累加 否则无视
           if (base.enter(token)) {
               enterCount++;
           }
           //进入成功 发送房间信息给进入的玩家 并通知在房间内的其他玩家 有人进入了
           SelectRoomDTO dto = new SelectRoomDTO();
           dto.teamOne = team.Values.ToArray();
           write(token, SelectProtocol.ENTER_SRES, dto);
           brocast(SelectProtocol.ENTER_EXBRO, userId, token);
       }

       public override byte GetType()
       {
           
           return Protocol.TYPE_SELECT;
       }
    }
}
