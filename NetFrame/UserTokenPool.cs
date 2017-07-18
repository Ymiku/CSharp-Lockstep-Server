﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
   public class UserTokenPool
    {
       private Stack<UserToken> pool;

       public UserTokenPool(int max) {
           pool = new Stack<UserToken>(max);
       }
       /// <summary>
       /// 取出一个连接对象 --创建连接
       /// </summary>
       public UserToken pop() {

           return pool.Pop();
       }
       //插入一个连接对象---释放连接
       public void push(UserToken token) {
           if (token != null)
               pool.Push(token);
       }
       public int Size {
           get { return pool.Count; } 
       }
    }
}
