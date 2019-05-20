/*******************************************************************************
 * Copyright © 2016 BFM.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.BFM.cn
*********************************************************************************/
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BFM.Web.Base.WebControls.Tree;
using BFM.Web.Base.WebControls.TreeGrid;
using BFM.Web.Base.WebControls.TreeView;

namespace BFM.Web.Areas.SystemManage.Controllers
{
    public class OrganizeController : ControllerBase
    {
        //private OrganizeApp organizeApp = new OrganizeApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            //var data = organizeApp.GetList();
            var treeList = new List<TreeSelectModel>();
            //foreach (OrganizeEntity item in data)
            //{
            //    TreeSelectModel treeModel = new TreeSelectModel();
            //    treeModel.id = item.F_Id;
            //    treeModel.text = item.F_FullName;
            //    treeModel.parentId = item.F_ParentId;
            //    treeModel.data = item;
            //    treeList.Add(treeModel);
            //}
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeJson()
        {
            //var data = organizeApp.GetList();
            var treeList = new List<TreeViewModel>();
            //foreach (OrganizeEntity item in data)
            //{
            //    TreeViewModel tree = new TreeViewModel();
            //    bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
            //    tree.id = item.F_Id;
            //    tree.text = item.F_FullName;
            //    tree.value = item.F_EnCode;
            //    tree.parentId = item.F_ParentId;
            //    tree.isexpand = true;
            //    tree.complete = true;
            //    tree.hasChildren = hasChildren;
            //    treeList.Add(tree);
            //}
            return Content(treeList.TreeViewJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            //var data = organizeApp.GetList();
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    data = data.TreeWhere(t => t.F_FullName.Contains(keyword));
            //}
            var treeList = new List<TreeGridModel>();
            //foreach (OrganizeEntity item in data)
            //{
            //    TreeGridModel treeModel = new TreeGridModel();
            //    bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
            //    treeModel.id = item.F_Id;
            //    treeModel.isLeaf = hasChildren;
            //    treeModel.parentId = item.F_ParentId;
            //    treeModel.expanded = hasChildren;
            //    treeModel.entityJson = item.ToJson();
            //    treeList.Add(treeModel);
            //}
            return Content(treeList.TreeGridJson());
        }
    }
}
