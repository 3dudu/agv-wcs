using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Comoon
{
    /// <summary>
    /// 操作返回
    /// </summary>
    public class OperateReturnInfo
    {
        OperateCodeEnum returnCode;
        object returnInfo;
        string adviceInfo;
        /// <summary>
        /// 构造函数
        /// </summary>
        public OperateReturnInfo()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="returnCode">OperateCodeEnum类型</param>
        public OperateReturnInfo(OperateCodeEnum returnCode)
        {
            this.returnCode = returnCode;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="returnCode">OperateCodeEnum类型</param>
        /// <param name="returnInfo">强制返回值</param>
        public OperateReturnInfo(OperateCodeEnum returnCode, object returnInfo)
        {
            this.returnCode = returnCode;
            this.returnInfo = returnInfo;
        }
        /// <summary>
        /// 返回的值类型
        /// </summary>
        public OperateCodeEnum ReturnCode
        {
            get { return returnCode; }
            set { returnCode = value; }
        }

        /// <summary>
        /// 返回的可选信息
        /// </summary>
        public object ReturnInfo
        {
            get
            {
                if (returnInfo == null) { return string.Empty; }
                else { return returnInfo; }
            }
            set { returnInfo = value; }
        }

        /// <summary>
        /// 建议信息
        /// </summary>
        public string AdviceInfo
        {
            get { return adviceInfo; }
            set { adviceInfo = value; }
        }
    }


    /// <summary>
    /// 操作返回枚举
    /// </summary>
    public enum OperateCodeEnum
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownError = 0,

        /// <summary>
        /// 操作成功
        /// </summary>
        Success,

        /// <summary>
        /// {0}失败
        /// </summary>
        Failed,

        /// <summary>
        /// 编码已经存在
        /// </summary>
        CodeIsExist,

        /// <summary>
        /// 编码长度不正确
        /// </summary>
        CodeLengthIsWrong,

        /// <summary>
        /// 保存编码长度时出错
        /// </summary>
        SaveCodeLengthError,

        /// <summary>
        /// 修改或者删除的对象不存在
        /// </summary>
        IsNotExist,

        /// <summary>
        /// 删除的对象已经被使用
        /// </summary>
        IsUsed,
    }
}
