using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace SpaceNetServer.dao.model
{
	/// <summary>
	/// 类ACCOUNT。
	/// </summary>
	[Serializable]
	public partial class ACCOUNT
	{
		public ACCOUNT()
		{}
		#region Model
		private int _id=-1;
		private string _account;
		private string _password;
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
		public string account
		{
			set{ _account=value;}
			get{return _account;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string password
		{
			set{ _password=value;}
			get{return _password;}
		}
		#endregion Model


		#region  Method

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public ACCOUNT(string account)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,account,password ");
			strSql.Append(" FROM ACCOUNT ");
            strSql.Append(" where account=@account ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@account", MySqlDbType.VarChar)};
            parameters[0].Value = account;

			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					this.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["account"]!=null)
				{
					this.account=ds.Tables[0].Rows[0]["account"].ToString();
				}
				if(ds.Tables[0].Rows[0]["password"]!=null)
				{
					this.password=ds.Tables[0].Rows[0]["password"].ToString();
				}
			}
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from ACCOUNT");
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
			strSql.Append("insert into ACCOUNT (");
			strSql.Append("account,password)");
			strSql.Append(" values (");
			strSql.Append("@account,@password)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@account", MySqlDbType.VarChar,255),
					new MySqlParameter("@password", MySqlDbType.VarChar,255)};
			parameters[0].Value = account;
			parameters[1].Value = password;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
            getKey();
		}


        void getKey()
        {
            DataSet ds = DbHelperMySQL.Query("select @@IDENTITY as id");
            if (ds.Tables[0].Rows[0]["id"] != null && ds.Tables[0].Rows[0]["id"].ToString() != string.Empty)
            {
                this._id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
            }
        }
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update()
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ACCOUNT set ");
			strSql.Append("account=@account,");
			strSql.Append("password=@password");
			strSql.Append(" where id=@id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@account", MySqlDbType.VarChar,255),
					new MySqlParameter("@password", MySqlDbType.VarChar,255),
					new MySqlParameter("@id", MySqlDbType.Int32,11)};
			parameters[0].Value = account;
			parameters[1].Value = password;
			parameters[2].Value = id;

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
			strSql.Append("delete from ACCOUNT ");
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
			strSql.Append("select id,account,password ");
			strSql.Append(" FROM ACCOUNT ");
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
				if(ds.Tables[0].Rows[0]["account"]!=null )
				{
					this.account=ds.Tables[0].Rows[0]["account"].ToString();
				}
				if(ds.Tables[0].Rows[0]["password"]!=null )
				{
					this.password=ds.Tables[0].Rows[0]["password"].ToString();
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
			strSql.Append(" FROM ACCOUNT ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperMySQL.Query(strSql.ToString());
		}

		#endregion  Method
	}
}

