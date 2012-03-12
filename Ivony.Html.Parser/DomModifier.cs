﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ivony.Fluent;

namespace Ivony.Html.Parser
{


  /// <summary>
  /// DOM 结构修改器
  /// </summary>
  public class DomModifier : IHtmlDomModifier, INotifyDomChanged
  {

    /// <summary>
    /// 修改文档的 URI
    /// </summary>
    /// <param name="document">要修改的文档</param>
    /// <param name="uri">新的文档 URI</param>
    public void ResolveUri( IHtmlDocument document, Uri uri )
    {

      var domDocument = document as DomDocument;

      if ( domDocument == null )
        throw new InvalidOperationException();

      domDocument.DocumentUri = uri;
    }

    /// <summary>
    /// 在容器指定位置添加一个元素
    /// </summary>
    /// <param name="container">要添加元素的容器</param>
    /// <param name="index">要添加元素的位置</param>
    /// <param name="name">添加元素的名称</param>
    /// <returns>添加好的元素</returns>
    public IHtmlElement AddElement( IHtmlContainer container, int index, string name )
    {
      var element = DomProvider.EnsureDomContainer( container ).InsertNode( index, new DomElement( name, null ) );

      OnDomChanged( this, new HtmlDomChangedEventArgs( element, container, HtmlDomChangedAction.Add ) );

      return element;
    }


    /// <summary>
    /// 在容器指定位置添加一个文本节点
    /// </summary>
    /// <param name="container">要添加元素的容器</param>
    /// <param name="index">要添加文本节点的位置</param>
    /// <param name="name">添加的 HTML 文本</param>
    /// <returns>添加好的文本节点</returns>
    public IHtmlTextNode AddTextNode( IHtmlContainer container, int index, string htmlText )
    {
      var textNode = DomProvider.EnsureDomContainer( container ).InsertNode( index, new DomTextNode( htmlText ) );

      OnDomChanged( this, new HtmlDomChangedEventArgs( textNode, container, HtmlDomChangedAction.Add ) );

      return textNode;
    }

    /// <summary>
    /// 在容器指定位置添加一个注释节点
    /// </summary>
    /// <param name="container">要添加注释的容器</param>
    /// <param name="index">要添加注释的位置</param>
    /// <param name="name">添加注释的内容</param>
    /// <returns>添加好的注释节点</returns>
    public IHtmlComment AddComment( IHtmlContainer container, int index, string comment )
    {
      var commentNode = DomProvider.EnsureDomContainer( container ).InsertNode( index, new DomComment( comment ) );

      OnDomChanged( this, new HtmlDomChangedEventArgs( commentNode, container, HtmlDomChangedAction.Add ) );

      return commentNode;
    }

    /// <summary>
    /// 在容器指定位置添加一个特殊节点
    /// </summary>
    /// <param name="container">要添加特殊节点的容器</param>
    /// <param name="index">要添加特殊节点的位置</param>
    /// <param name="name">添加特殊节点的 HTML</param>
    /// <returns>添加好的特殊节点</returns>
    public IHtmlSpecial AddSpecial( IHtmlContainer container, int index, string html )
    {
      var specialNode = DomProvider.EnsureDomContainer( container ).InsertNode( index, new DomSpecial( html ) );

      //UNDONE 未确定special node具体是什么

      OnDomChanged( this, new HtmlDomChangedEventArgs( specialNode, container, HtmlDomChangedAction.Add ) );

      return specialNode;
    }

    /// <summary>
    /// 移除一个节点
    /// </summary>
    /// <param name="node">要移除的节点</param>
    public void RemoveNode( IHtmlNode node )
    {
      var container = node.Container;

      EnsureDomNode( node ).Remove();

      OnDomChanged( this, new HtmlDomChangedEventArgs( node, container, HtmlDomChangedAction.Remove ) );
    }


    private DomNode EnsureDomNode( IHtmlNode node )
    {
      var domNode = node as DomNode;

      if ( domNode == null )
        throw new NotSupportedException( "只能操作特定类型节点" );

      return domNode;
    }


    /// <summary>
    /// 为指定元素添加一个属性
    /// </summary>
    /// <param name="element">要添加属性的元素</param>
    /// <param name="name">属性名称</param>
    /// <param name="value">属性值</param>
    /// <returns>添加好的属性</returns>
    public IHtmlAttribute AddAttribute( IHtmlElement element, string name, string value )
    {
      var domElement = element as DomElement;
      if ( domElement == null )
        throw new InvalidOperationException();

      return domElement.AddAttribute( name, value );
    }


    /// <summary>
    /// 移除指定的属性
    /// </summary>
    /// <param name="attribute">要移除的属性</param>
    public void RemoveAttribute( IHtmlAttribute attribute )
    {
      var domAttribute = attribute as DomAttribute;
      if ( domAttribute == null )
        throw new InvalidOperationException();

      domAttribute.Remove();
    }



    internal void OnFragmentInto( DomFragment fragment, IHtmlContainer targetContainer, DomNode node )
    {
      OnDomChanged( this, new HtmlDomChangedEventArgs( node, fragment, HtmlDomChangedAction.Remove ) );
      OnDomChanged( this, new HtmlDomChangedEventArgs( node, targetContainer, HtmlDomChangedAction.Add ) );
    }


    /// <summary>
    /// 引发 DomChanged 事件
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">时间参数</param>
    protected virtual void OnDomChanged( object sender, HtmlDomChangedEventArgs e )
    {
      if ( HtmlDomChanged != null )
        HtmlDomChanged( sender, e );
    }

    /// <summary>
    /// 当 HTML DOM 结构发生改变时引发此事件
    /// </summary>
    public event EventHandler<HtmlDomChangedEventArgs> HtmlDomChanged;
  }
}
