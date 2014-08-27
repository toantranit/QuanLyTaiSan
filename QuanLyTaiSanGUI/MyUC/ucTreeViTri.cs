﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using QuanLyTaiSan.Entities;
using QuanLyTaiSan.DataFilter;
using QuanLyTaiSanGUI.MyUserControl;
using QuanLyTaiSanGUI.QLThietBi;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Localization;

namespace QuanLyTaiSanGUI.MyUC
{
    public partial class ucTreeViTri : UserControl
    {
        private Guid phongid = Guid.Empty;
        private Guid cosoid = Guid.Empty;
        private Guid dayid = Guid.Empty;
        private Guid tangid = Guid.Empty;

        public delegate void SetData_phongid(Guid id);
        public SetData_phongid setData_phongid = null;

        public delegate void FocusedRow_phong();
        public FocusedRow_phong focusedRow_phong = null;

        public ucTreeViTri()
        {
            InitializeComponent();
        }

        public void loadData(List<ViTriHienThi> _list)
        {
            treeListViTri.BeginUnboundLoad();
            treeListViTri.DataSource = _list;
            treeListViTri.EndUnboundLoad();
        }

        private void treeListPhong_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                phongid = Guid.Empty;
                cosoid = Guid.Empty;
                dayid = Guid.Empty;
                tangid = Guid.Empty;
                if(e.Node.GetValue(2)!= null)
                {
                    switch (e.Node.GetValue(2).ToString())
                    {
                        case "CoSo":
                            cosoid = QuanLyTaiSan.Libraries.GUID.From(e.Node.GetValue(0).ToString());
                            break;
                        case "Dayy":
                            dayid = QuanLyTaiSan.Libraries.GUID.From(e.Node.GetValue(0).ToString());
                            cosoid = QuanLyTaiSan.Libraries.GUID.From(e.Node.ParentNode.GetValue(0).ToString());
                            break;
                        case "Tang":
                            tangid = QuanLyTaiSan.Libraries.GUID.From(e.Node.GetValue(0).ToString());
                            dayid = QuanLyTaiSan.Libraries.GUID.From(e.Node.ParentNode.GetValue(0).ToString());
                            cosoid = QuanLyTaiSan.Libraries.GUID.From(e.Node.ParentNode.ParentNode.GetValue(0).ToString());
                            break;
                        case "Phong":
                            phongid = QuanLyTaiSan.Libraries.GUID.From(e.Node.GetValue(0).ToString());
                            break;
                    }
                    if (setData_phongid != null)
                        setData_phongid(phongid);
                    if (focusedRow_phong != null && (cosoid != Guid.Empty || dayid != Guid.Empty || tangid != Guid.Empty))
                        focusedRow_phong();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->treeListPhong_FocusedNodeChanged: " + ex.Message);
            }
        }

        public ViTri getVitri()
        {
            try
            {
                ViTri obj = new ViTri();
                obj.coso = CoSo.getById(cosoid);
                obj.day = Dayy.getById(dayid);
                obj.tang = Tang.getById(tangid);
                return obj;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this.Name + "->getVitri: " + ex.Message);
                return null;
            }
            finally
            { }
        }

        public Phong getPhong()
        {
            if (phongid != Guid.Empty)
                return Phong.getById(phongid);
            else
                return new Phong();
        }

        public TreeList getTreeList()
        {
            return treeListViTri;
        }

        public void setVitri(ViTri obj)
        {
            try
            {
                if (obj != null && obj.id != Guid.Empty)
                {
                    FindNode findNode = null;
                    if (obj.tang != null)
                    {
                        findNode = new FindNode(obj.tang.id, typeof(Tang).Name);
                    }
                    else if (obj.day != null)
                    {
                        findNode = new FindNode(obj.day.id, typeof(Dayy).Name);
                    }
                    else if (obj.coso != null)
                    {
                        findNode = new FindNode(obj.coso.id, typeof(CoSo).Name);
                    }
                    if (findNode != null)
                    {
                        treeListViTri.CollapseAll();
                        treeListViTri.NodesIterator.DoOperation(findNode);
                        treeListViTri.FocusedNode = findNode.Node;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : setVitri : " + ex.Message);
            }
            finally
            { }
        }

        public void setPhong(Phong obj)
        {
            try
            {
                if (obj != null && obj.id != Guid.Empty)
                {
                    FindNode findNode = new FindNode(obj.id, typeof(Phong).Name);
                    treeListViTri.CollapseAll();
                    treeListViTri.NodesIterator.DoOperation(findNode);
                    treeListViTri.FocusedNode = findNode.Node;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(this.Name + " : setPhong : " + ex.Message);
            }
            finally
            { }
        }

        private void OnFilterNode(object sender, FilterNodeEventArgs e)
        {
            List<TreeListColumn> filteredColumns = e.Node.TreeList.Columns.Cast<TreeListColumn>(
                ).ToList();
            if (filteredColumns.Count == 0) return;
            if (string.IsNullOrEmpty(treeListViTri.FindFilterText)) return;
            e.Handled = true;
            e.Node.Visible = filteredColumns.Any(c => IsNodeMatchFilter(e.Node, c));
            e.Node.Expanded = e.Node.Visible;
        }

        bool IsNodeMatchFilter(TreeListNode node, TreeListColumn column)
        {
            string filterValue = treeListViTri.FindFilterText;
            if (node.GetDisplayText(column).ToUpper().Contains(filterValue.ToUpper())) return true;
            foreach (TreeListNode n in node.Nodes)
                if (IsNodeMatchFilter(n, column)) return true;
            return false;
        }

    }
}
