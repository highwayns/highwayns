using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using NC.HPS.Lib;

namespace HPSManagement
{
    /// <summary>
    /// 反射类    
    /// </summary>         
    public class MReference    
    {        
        /// <summary>        
        /// 得到类型        
        /// </summary>        
        /// <param name="consultClass">与要反射的类同一命名空间下的别一类型,用于得到完全限定名</param>        
        /// <param name="targetClassName">需要反射的类名</param>        
        /// <returns>反射得到的类型</returns>        
        public static Type getType(Type consultClass,string targetClassName)        
        {            
            //得到完全限定名            
            string assemblyQualifiedName = consultClass.AssemblyQualifiedName;            
            //从完全限定名的第一个逗号开始得到完全限定名的公共部份            
            string assemblyInformation = assemblyQualifiedName.Substring(assemblyQualifiedName.IndexOf(","));            
            try            
            {                
                //根据要反射的类名+截取后的限定名得到要反射类的类型                
                Type ty = Type.GetType(targetClassName + assemblyInformation);                
                return ty;            
            }            
            catch (Exception ex)            
            {                
                return null;            
            }                   
        }    
    }
    public class FormFactory
    {

        /// <summary>        
        /// 根据传入的数据库名,返回一个表单的实例        
        /// </summary>        
        /// <param name="tableName">数据库的表名</param>        
        /// <returns>与此表相对应的表单对象</returns>        
        public static Form GetInstance(string tableName, CmWinServiceAPI db)        
        {
            //与目标对象类型相同命名空间的类型            
            Type sourceType = typeof(HPSManagement.FormMain);            
            //目标对象类型全名(包括命名空间)            
            string className = "HPSManagement." + tableName;            
            //如果表名为空抛出异常            
            if (string.IsNullOrEmpty(tableName))                
                throw new ArgumentNullException("tableName");            
            return GetInstance(sourceType, className,db) as Form;        
        }        
        /// <summary>        
        /// 得到对象        
        /// </summary>        
        /// <param name="sourceType">与目标对象类型相同命名空间的类型</param>        
        /// <param name="className">//目标对象类型全名(包括命名空间)</param>        
        /// <returns>反射得到的对象</returns>        
        private static object GetInstance(Type sourceType, string className, CmWinServiceAPI db)        
        {            
            try            
            {                
                //根据传入的表名得到对应窗体的类型 MasterSoft.WinUI.frmMain是相同命名空间下的一个类型,用于得到限定名                
                Type ty = MReference.getType(sourceType, className);                
                if (ty == null)                    
                    throw new ArgumentNullException("ty");                
                //得到类型的 GetInstance 方法(当然这个类型必须有这个静态方法-单例模式)                
                MethodInfo getInstance = ty.GetMethod("GetInstance");                
                if (getInstance == null)                    
                    throw new ArgumentNullException("getInstance");                
                //调用GetInstance静态方法                
                object obj = (Form)getInstance.Invoke(null, new object[]{db});                
                //IMdiChildEditForm frm = ((IMdiChildEditForm)System.Activator.CreateInstance(ty)).GetInstance();                
                //返回得到的窗体                
                return obj;            
            }            
            catch (Exception ex)            
            {                
                //throw ex;            
            }
            return null;
        }
    }
}
