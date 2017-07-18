using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;
using NetFrame;//Please add references
namespace SpaceNetServer.dao.model
{
	/// <summary>
	/// 类USER。
	/// </summary>
	[Serializable]
	public partial class USER
	{
		public USER()
		{}
		#region Model
		private int _id=-1;
		private string _name;
		private int _level=1;
		private int _exp=0;
		private int _win=0;
        private int _lose = 0;
        private int _ran = 0;
		private int _accountid;
		private int[] _herolist;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int level
		{
			set{ _level=value;}
			get{return _level;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int exp
		{
			set{ _exp=value;}
			get{return _exp;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int win
		{
			set{ _win=value;}
			get{return _win;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int lose
		{
			set{ _lose=value;}
			get{return _lose;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ran
		{
			set{ _ran=value;}
			get{return _ran;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int accountId
		{
			set{ _accountid=value;}
			get{return _accountid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int[] heroList
		{
			set{ _herolist=value;}
			get{return _herolist;}
		}

		#endregion Model


		#region  Method

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public USER(int accountId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,name,level,exp,win,lose,ran,accountId,heroList ");
			strSql.Append(" FROM USER ");
            strSql.Append(" where accountId=@accountId ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@accountId", MySqlDbType.Int32)};
            parameters[0].Value = accountId;

			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					this.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["name"]!=null)
				{
					this.name=ds.Tables[0].Rows[0]["name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["level"]!=null && ds.Tables[0].Rows[0]["level"].ToString()!="")
				{
					this.level=int.Parse(ds.Tables[0].Rows[0]["level"].ToString());
				}
				if(ds.Tables[0].Rows[0]["exp"]!=null && ds.Tables[0].Rows[0]["exp"].ToString()!="")
				{
					this.exp=int.Parse(ds.Tables[0].Rows[0]["exp"].ToString());
				}
				if(ds.Tables[0].Rows[0]["win"]!=null && ds.Tables[0].Rows[0]["win"].ToString()!="")
				{
					this.win=int.Parse(ds.Tables[0].Rows[0]["win"].ToString());
				}
				if(ds.Tables[0].Rows[0]["lose"]!=null && ds.Tables[0].Rows[0]["lose"].ToString()!="")
				{
					this.lose=int.Parse(ds.Tables[0].Rows[0]["lose"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ran"]!=null && ds.Tables[0].Rows[0]["ran"].ToString()!="")
				{
					this.ran=int.Parse(ds.Tables[0].Rows[0]["ran"].ToString());
				}
				if(ds.Tables[0].Rows[0]["accountId"]!=null && ds.Tables[0].Rows[0]["accountId"].ToString()!="")
				{
					this.accountId=int.Parse(ds.Tables[0].Rows[0]["accountId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["heroList"]!=null && ds.Tables[0].Rows[0]["heroList"].ToString()!="")
				{
					this.heroList=(int[])SerializeUtil.decode( (byte[])ds.Tables[0].Rows[0]["heroList"]);
				}
			}
		}


		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from USER");
			strSql.Append(" where id=@id ");

			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)};
			parameters[0].Value = id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add()
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into USER (");
			strSql.Append("name,level,exp,win,lose,ran,accountId,heroList)");
			strSql.Append(" values (");
			strSql.Append("@name,@level,@exp,@win,@lose,@ran,@accountId,@heroList)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@name", MySqlDbType.VarChar,255),
					new MySqlParameter("@level", MySqlDbType.Int32,4),
					new MySqlParameter("@exp", MySqlDbType.Int32,11),
					new MySqlParameter("@win", MySqlDbType.Int32,11),
					new MySqlParameter("@lose", MySqlDbType.Int32,11),
					new MySqlParameter("@ran", MySqlDbType.Int32,11),
					new MySqlParameter("@accountId", MySqlDbType.Int32,11),
					new MySqlParameter("@heroList", MySqlDbType.VarBinary)};
			parameters[0].Value = name;
			parameters[1].Value = level;
			parameters[2].Value = exp;
			parameters[3].Value = win;
			parameters[4].Value = lose;
			parameters[5].Value = ran;
			parameters[6].Value = accountId;
			parameters[7].Value = SerializeUtil.encode(heroList);

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
            getKey();
		}

        void getKey() {
            DataSet ds = DbHelperMySQL.Query("select @@IDENTITY as id");
            if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!=string.Empty){
                this._id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            }
        }
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update()
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update USER set ");
			strSql.Append("name=@name,");
			strSql.Append("level=@level,");
			strSql.Append("exp=@exp,");
			strSql.Append("win=@win,");
			strSql.Append("lose=@lose,");
			strSql.Append("ran=@ran,");
			strSql.Append("accountId=@accountId,");
			strSql.Append("heroList=@heroList");
			strSql.Append(" where id=@id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@name", MySqlDbType.VarChar,255),
					new MySqlParameter("@level", MySqlDbType.Int32,4),
					new MySqlParameter("@exp", MySqlDbType.Int32,11),
					new MySqlParameter("@win", MySqlDbType.Int32,11),
					new MySqlParameter("@lose", MySqlDbType.Int32,11),
					new MySqlParameter("@ran", MySqlDbType.Int32,11),
					new MySqlParameter("@accountId", MySqlDbType.Int32,11),
					new MySqlParameter("@heroList", MySqlDbType.VarBinary),
					new MySqlParameter("@id", MySqlDbType.Int32,11)};
			parameters[0].Value = name;
			parameters[1].Value = level;
			parameters[2].Value = exp;
			parameters[3].Value = win;
			parameters[4].Value = lose;
			parameters[5].Value = ran;
			parameters[6].Value = accountId;
			parameters[7].Value = SerializeUtil.encode(heroList);
			parameters[8].Value = id;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from USER ");
			strSql.Append(" where id=@id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)};
			parameters[0].Value = id;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public void GetModel(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,name,level,exp,win,lose,ran,accountId,heroList ");
			strSql.Append(" FROM USER ");
			strSql.Append(" where id=@id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@id", MySqlDbType.Int32)};
			parameters[0].Value = id;

			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					this.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["name"]!=null )
				{
					this.name=ds.Tables[0].Rows[0]["name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["level"]!=null && ds.Tables[0].Rows[0]["level"].ToString()!="")
				{
					this.level=int.Parse(ds.Tables[0].Rows[0]["level"].ToString());
				}
				if(ds.Tables[0].Rows[0]["exp"]!=null && ds.Tables[0].Rows[0]["exp"].ToString()!="")
				{
					this.exp=int.Parse(ds.Tables[0].Rows[0]["exp"].ToString());
				}
				if(ds.Tables[0].Rows[0]["win"]!=null && ds.Tables[0].Rows[0]["win"].ToString()!="")
				{
					this.win=int.Parse(ds.Tables[0].Rows[0]["win"].ToString());
				}
				if(ds.Tables[0].Rows[0]["lose"]!=null && ds.Tables[0].Rows[0]["lose"].ToString()!="")
				{
					this.lose=int.Parse(ds.Tables[0].Rows[0]["lose"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ran"]!=null && ds.Tables[0].Rows[0]["ran"].ToString()!="")
				{
					this.ran=int.Parse(ds.Tables[0].Rows[0]["ran"].ToString());
				}
				if(ds.Tables[0].Rows[0]["accountId"]!=null && ds.Tables[0].Rows[0]["accountId"].ToString()!="")
				{
					this.accountId=int.Parse(ds.Tables[0].Rows[0]["accountId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["heroList"]!=null && ds.Tables[0].Rows[0]["heroList"].ToString()!="")
				{
                    this.heroList = (int[])SerializeUtil.decode((byte[])ds.Tables[0].Rows[0]["heroList"]);
				}
			}
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * ");
			strSql.Append(" FROM USER ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperMySQL.Query(strSql.ToString());
		}

		#endregion  Method
	}
}

