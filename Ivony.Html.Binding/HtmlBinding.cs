﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ivony.Html.Binding
{


  public class HtmlBinding
  {


    public static StyleBinder StyleBinder { get; private set; }
    public static LiteralBinder LiteralBinder { get; private set; }
    public static FormBinder FormBinder { get; private set; }
    public static ScriptBinder ScriptBinder { get; private set; }
    public static BindingExpressionBinder BindingExpressionBinder { get; private set; }


    static HtmlBinding()
    {
      StyleBinder = new StyleBinder();
      BindingExpressionBinder = new BindingExpressionBinder();

      FormBinder = new FormBinder();
      ScriptBinder = new ScriptBinder();
      LiteralBinder = new LiteralBinder();
    }


    /// <summary>
    /// 使用默认的绑定器设置创建 HtmlBindingContext 实例
    /// </summary>
    /// <param name="scope">要进行数据绑定的范畴</param>
    /// <param name="dataContext">数据上下文</param>
    /// <param name="dataValues">数据字典</param>
    public static HtmlBindingContext Create( IHtmlContainer scope, object dataContext, IDictionary<string, object> dataValues )
    {
      return HtmlBindingContext.Create( new IHtmlBinder[] { StyleBinder, LiteralBinder }, new IExpressionBinder[] { BindingExpressionBinder }, scope, dataContext, dataValues );
    }



    /// <summary>
    /// 使用默认的绑定器设置进行数据绑定
    /// </summary>
    /// <param name="scope">要进行数据绑定的范畴</param>
    /// <param name="dataContext">数据上下文</param>
    /// <param name="dataValues">数据字典</param>
    public static void DataBind( IHtmlContainer scope, object dataContext, IDictionary<string, object> dataValues )
    {
      Create( scope, dataContext, dataValues ).DataBind();
    }
  }
}
