using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RCSEditTool.FrmCommon;
using Tools;
using Model.MDM;
using Model.MSM;
using Model.Comoon;

namespace RCSEditTool.FrmExternal
{
    public partial class FrmFragmentSet : BaseForm
    {
        string Fragment = "";
        int Set = 0;
        public FrmFragmentSet(string fragment, int set)
        {
            InitializeComponent();
            Fragment = fragment;
            Set = set;
        }
        /// <summary>
        /// 新增一条动作指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                gvCmdSet.CloseEditor();
                gvActionLandMark.CloseEditor();
                CmdInfo cmd = bsCmd.Current as CmdInfo;
                LandmarkToActionsInfo frag = bsAction.Current as LandmarkToActionsInfo;
                if (frag != null)
                {
                    if (cmd != null)
                    {

                        foreach (var item in frag.ActionList)
                        {
                            if (cmd.CmdCode == item.CmdCode)
                            {
                                return;
                            }
                        }
                    }
                    frag.ActionList.Add(new RouteFragmentConfigInfo()
                    {
                        CmdCode = cmd.CmdCode,
                        CmdName = cmd.CmdName,
                    });
                    this.bsFragment.DataSource = frag.ActionList;
                    this.bsFragment.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 删除一条动作指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {

            try
            {
                gvCmdSet.CloseEditor();
                RouteFragmentConfigInfo currFrag = bsFragment.Current as RouteFragmentConfigInfo;
                if (currFrag == null) { return; }
                bsFragment.Remove(currFrag);
                bsFragment.ResetBindings(false);

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 向上移动一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvCmdSet.IsFirstRow) { return; }


                RouteFragmentConfigInfo frag = bsFragment.Current as RouteFragmentConfigInfo;
                if (frag == null) { return; }
                IList<RouteFragmentConfigInfo> listfrag = bsFragment.List as IList<RouteFragmentConfigInfo>;
                if (listfrag == null) { return; }
                if (listfrag.Count <= 0) { return; }
                int selectIndex = listfrag.IndexOf(frag);
                RouteFragmentConfigInfo pre = DataToObject.CreateDeepCopy<RouteFragmentConfigInfo>(listfrag[selectIndex - 1]);
                frag.CmdIndex = frag.CmdIndex - 1;
                listfrag[selectIndex - 1] = frag;
                pre.CmdIndex = pre.CmdIndex + 1;
                listfrag[selectIndex] = pre;

                bsFragment.DataSource = listfrag;
                bsFragment.ResetBindings(false);
                gvCmdSet.MovePrev();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }/// <summary>
         /// 向下移动一行
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvCmdSet.IsLastRow) { return; }
                RouteFragmentConfigInfo currAct = bsFragment.Current as RouteFragmentConfigInfo;
                if (currAct == null) { return; }
                IList<RouteFragmentConfigInfo> acts = bsFragment.List as IList<RouteFragmentConfigInfo>;
                if (acts == null) { return; }
                if (acts.Count <= 0) { return; }
                int selectIndex = acts.IndexOf(currAct);
                RouteFragmentConfigInfo NextAct = DataToObject.CreateDeepCopy<RouteFragmentConfigInfo>(acts[selectIndex + 1]);
                NextAct.CmdIndex = NextAct.CmdIndex - 1;
                acts[selectIndex] = NextAct;
                currAct.CmdIndex = currAct.CmdIndex + 1;
                acts[selectIndex + 1] = currAct;
                bsFragment.DataSource = acts;
                bsFragment.ResetBindings(false);
                gvCmdSet.MoveNext();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void FrmFragmentSet_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 显示页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected internal void FrmFragmentSet_Shown(object sender, EventArgs e)
        {
            try
            {
                CenterToParent();
                this.bsCmd.DataSource = AGVDAccess.AGVClientDAccess.get_Cmd();
                RouteFragmentConfigInfo routefrag = new RouteFragmentConfigInfo();
                bsCmd.ResetBindings(false);
                if (Set == 1)
                {
                    routefrag.Fragment = Fragment;
                    txtFragment.Text = Fragment;
                    txtFragment.ReadOnly = true;
                    IList<RouteFragmentConfigInfo> RouteFragmentConfigList = AGVDAccess.AGVClientDAccess.load_OtherByFragment(Fragment);


                    IList<string> landmarks = new List<string>();
                    foreach (var item in RouteFragmentConfigList)
                    {
                        if (!landmarks.Contains(item.ActionLandMark))
                        {
                            landmarks.Add(item.ActionLandMark);
                        }
                    }
                    IList<LandmarkToActionsInfo> LandmarkToActionsList = new List<LandmarkToActionsInfo>();
                    foreach (var landmark in landmarks)
                    {
                        LandmarkToActionsInfo item = new LandmarkToActionsInfo();
                        item.LandCode = landmark;
                        foreach (var action in RouteFragmentConfigList)
                        {
                            if (action.ActionLandMark == landmark)
                            {
                                IList<CmdInfo> cmd = this.bsCmd.List as List<CmdInfo>;
                                foreach (var cmdName in cmd)
                                {
                                    if (action.CmdCode == cmdName.CmdCode)
                                    {
                                        action.CmdName = cmdName.CmdName;
                                    }

                                }
                                item.ActionList.Add(action);
                            }
                        }
                        LandmarkToActionsList.Add(item);
                    }
                    this.bsAction.DataSource = LandmarkToActionsList;
                }

                this.gvActionLandMark.RefreshData();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 退出当前页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }


        /// <summary>
        /// 右键添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCmAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (Valid())
                {
                    gvActionLandMark.CloseEditor();
                    LandmarkToActionsInfo LandmarkToActions = bsAction.Current as LandmarkToActionsInfo;
                    LandmarkToActionsInfo newLand = new LandmarkToActionsInfo();
                    bsAction.Add(newLand);
                    bsAction.ResetBindings(false);
                    bsAction.MoveLast();
                    gvActionLandMark.ShowEditor();

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 右键删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCmDel_Click(object sender, EventArgs e)
        {
            try
            {
                RouteFragmentConfigInfo fragconfig = bsFragment.Current as RouteFragmentConfigInfo;
                if (fragconfig == null) return;
                bsAction.Remove(fragconfig);
                bsAction.ResetBindings(false);
                gvActionLandMark_FocusedRowChanged(null, null);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 焦点行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvActionLandMark_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {

                LandmarkToActionsInfo currFrag = bsAction.Current as LandmarkToActionsInfo;

                if (currFrag == null) { return; }
                bsFragment.DataSource = currFrag.ActionList;
                bsFragment.ResetBindings(false);

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 验证数据正确性
        /// </summary>
        /// <returns></returns>
        private bool Valid()
        {
            try
            {
                gvActionLandMark.CloseEditor();
                gvCmd.CloseEditor();
                gvCmdSet.CloseEditor();
                IList<LandmarkToActionsInfo> Landmark = this.bsAction.List as IList<LandmarkToActionsInfo>;
                if (Landmark != null)
                {

                    foreach (var item in Landmark)
                    {
                        if (!txtFragment.Text.Contains("," + item.LandCode + ","))
                        {
                            MsgBox.ShowError("当前线路线段中未包含" + item.LandCode + "!");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 保存线路线段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!(txtFragment.Text.StartsWith(",") && txtFragment.Text.EndsWith(",")))
                {
                    MsgBox.ShowWarn("线路线段未以逗号开头或结尾!");
                    return;
                }
                if (Valid())
                {
                    gvActionLandMark.CloseEditor();
                    gvCmdSet.CloseEditor();
                    IList<RouteFragmentConfigInfo> RouteFragment = new List<RouteFragmentConfigInfo>();
                    IList<LandmarkToActionsInfo> landmark = bsAction.List as IList<LandmarkToActionsInfo>;
                    foreach (var Action in landmark)
                    {
                        int index = 0;
                        foreach (var land in Action.ActionList)
                        {
                            RouteFragment.Add(new RouteFragmentConfigInfo()
                            {
                                CmdIndex = index,
                                CmdCode = land.CmdCode,
                                CmdPara = land.CmdPara,
                                Fragment = txtFragment.Text.Trim(),
                                ActionLandMark = Action.LandCode
                            });
                            index++;
                        }
                    }


                    OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.save_tbRouteFragmentConfig(RouteFragment);
                    MsgBox.Show(opr);
                    if (opr.ReturnCode == OperateCodeEnum.Success)
                    {
                        this.DialogResult = DialogResult.OK;
                        //this.Parent.Refresh();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 双击添加动作指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvCmd_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                gvCmdSet.CloseEditor();
                gvActionLandMark.CloseEditor();
                CmdInfo cmd = bsCmd.Current as CmdInfo;
                LandmarkToActionsInfo frag = bsAction.Current as LandmarkToActionsInfo;
                foreach (var item in frag.ActionList)
                {
                    if (cmd.CmdCode == item.CmdCode)
                    {
                        return;
                    }
                }
                frag.ActionList.Add(new RouteFragmentConfigInfo()
                {
                    CmdCode = cmd.CmdCode,
                    CmdName = cmd.CmdName,
                });
                this.bsFragment.DataSource = frag.ActionList;
                this.bsFragment.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        /// <summary>
        /// 双击删除动作指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvCmdSet_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                gvCmdSet.CloseEditor();
                RouteFragmentConfigInfo currFrag = bsFragment.Current as RouteFragmentConfigInfo;
                if (currFrag == null) { return; }
                bsFragment.Remove(currFrag);
                bsFragment.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
    }
}