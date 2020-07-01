using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace SPTextCommon.Html
{
    public static class ControlsExtensionsHelper
    {
        public static MvcHtmlString Labels(this HtmlHelper htmlHelper, string text, string id)
        {

            string str = String.Format("<label id='{0}' >{1}</label>", id, text);

            return new MvcHtmlString(str);

        }

        /// <summary>
        /// 查询按钮控件
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString CreateSearch(this HtmlHelper htmlHelper)
        {

            string str = String.Format("<input type='submit' class='btn_search'  id='btnsearch' value='查询'  accesskey='S' />");

            return new MvcHtmlString(str);

        }

        /// <summary>
        /// 文本框
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString CreateText(this HtmlHelper htmlHelper, string name)
        {
            string str = String.Format("<input type='text' name='{0}' id='{0}' class='text' style='width:120px;margin-right:4px;margin-left:2px' />", name);

            return new MvcHtmlString(str);

        }

        /// <summary>
        /// 文本框
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString Create140PxText(this HtmlHelper htmlHelper, string name)
        {
            string str = String.Format("<input type='text' name='{0}' id='{0}' class='text' style='width:140px;' />", name);

            return new MvcHtmlString(str);

        }

        /// <summary>
        /// 开始时间文本框
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString CreateStartDate(this HtmlHelper htmlHelper, string name)
        {
            string str = String.Format("<input type='text' name='{0}' id='{0}'   class='text date' style='width:120px'  onclick='WdatePicker()' />", name);


            return new MvcHtmlString(str);

        }
        /// <summary>
        /// 结束时间文本框
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString CreateEndDate(this HtmlHelper htmlHelper, string name)
        {
            string str = String.Format(" <input type='text' name='{0}' id='{0}'   class='text date' style='width:120px'   onclick='WdatePicker()'/>", name);

            return new MvcHtmlString(str);

        }

        /// <summary>
        /// 导出控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString CreateExport(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            string key = "class";
            string val = "btn_export";
            string value = "导出";
            return CreateButton(htmlHelper, name, value, htmlAttributes, key, val);
            //string str = String.Format("<input type='button' class='btn_export' id='{0}'  value='导出'  />",name); 

            //  return new MvcHtmlString(str); 

        }

        /// <summary>
        /// 导出控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString CreateExport(this HtmlHelper htmlHelper)
        {

            string name = "btn_expt";
            object obj = null;
            return CreateExport(htmlHelper, name, obj);

        }

        /// <summary>
        /// 导出控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString CreateExport(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            string name = "btn_expt";
            return CreateExport(htmlHelper, name, htmlAttributes);

        }



        public static MvcHtmlString CreateButton(this HtmlHelper htmlHelper, string name, string value)
        {
            object obj = null;
            return CreateButton(htmlHelper, name, value, obj);
        }

        /// <summary>
        /// 创建button按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CreateButton(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes)
        {
            string str1 = GetHtmlAttributes(htmlAttributes);
            string str = String.Format(" <input type='button' id='{0}' name='{0}' value='{1}'  {2} />", name, value, str1);

            return new MvcHtmlString(str);
        }

        /// <summary>
        ///  创建button按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="key">固定的key class</param>
        /// <param name="value2">固定的value btn_new</param>
        /// <returns></returns>
        private static MvcHtmlString CreateButton(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes, string key, string value2)
        {

            string str1 = GetHtmlAttributes(htmlAttributes, key, value2);
            string str = String.Format(" <input type='button' id='{0}' name='{0}' value='{1}'  {2} />", name, value, str1);

            return new MvcHtmlString(str);
        }

        #region 判断html特性标签
        private static RouteValueDictionary AnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    dictionary.Add(descriptor.Name.Replace('_', '-'), descriptor.GetValue(htmlAttributes));
                }
            }
            return dictionary;
        }
        //判断是否有html的特性标签 例:new {onclick=add()}
        private static string GetHtmlAttributes(object htmlAttributes)
        {
            IDictionary<string, object> dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            string str1 = "";
            if (dictionary != null && dictionary.Any())
            {
                foreach (var item in dictionary)
                {
                    str1 += item.Key + "=" + item.Value + "  ";
                }
            }
            return str1;
        }
        //判断是否有html的特性标签 例:new {onclick=add()} 
        private static string GetHtmlAttributes(object htmlAttributes, string key, string value)
        {
            IDictionary<string, object> dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            dictionary.Add(key, value);
            string str1 = "";
            if (dictionary != null && dictionary.Any())
            {
                foreach (var item in dictionary)
                {
                    str1 += item.Key + "=" + item.Value + "  ";
                }
            }
            return str1;
        }
        #endregion


        /// <summary>
        /// 创建一个清空的按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CreatebtnClear(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            string key = "class";
            string value = "btn_clean";
            return CreateButton(htmlHelper, "btnclear", "清空", htmlAttributes, key, value);

        }



        /// <summary>
        /// 创建一个编辑的按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CreatebtnEdit(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            string key = "class";
            string val = "btn_edit";
            string name = "btnEdit";
            string value = "编辑";
            return CreateButton(htmlHelper, name, value, htmlAttributes, key, val);

        }

        /// 创建一个删除的按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CreatebtnDelete(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            string key = "class";
            string val = "btn_remove";
            string name = "btnDel";
            string value = "删除";
            return CreateButton(htmlHelper, name, value, htmlAttributes, key, val);

        }

        /// 创建一个新增的按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CreatebtnAdd(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            string key = "class";
            string val = "btn_new";
            string name = "btn_new";
            string value = "新建";
            return CreateButton(htmlHelper, name, value, htmlAttributes, key, val);

        }


        /// <summary>
        /// Span标签
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MvcHtmlString Span(this HtmlHelper htmlHelper, string value)
        {
            string str = string.Format("<span>{0}</span>", value);
            return new MvcHtmlString(str);

        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString btnSave(this HtmlHelper htmlHelper)
        {

            string name = "btnSave";
            string value = "保存";
            string str = string.Format("<input class='btn_save' type='submit' id='{0}' name='{0}' value='{1}' />", name, value);
            return new MvcHtmlString(str);
        }



        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString btnClose(this HtmlHelper htmlHelper)
        {
            string name = "btnClose";
            string value = "关闭";
            string key = "class";
            string val = "btn_back";
            object obj = new { onclick = "helper.closeWin()" };
            return CreateButton(htmlHelper, name, value, obj, key, val);
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CreatebtnCancel(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            string key = "class";
            string val = "btn_uncheck";
            string name = "btn_uncheck";
            string value = "取消";
            return CreateButton(htmlHelper, name, value, htmlAttributes, key, val);
        }




    }
}
