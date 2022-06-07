using SPMVCText.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPMVCText.Controllers
{
    [AuthorizeFilter]
    public class ExerciseManagerController : Controller
    {
        //
        // GET: /ExerciseManager/
        [AuthorizeFilter]
        public ActionResult Index()
        {
            return View();
        }

        #region  获取练习列表为Jqgrid的数据
        public class GetExerciseInfoListForJqgridParam
        {
            public string ExerciseName { get; set; }
            public int rows { get; set; }
            public int page { get; set; }
        }
        [JsonException]
        public JsonResult GetExerciseInfoListForJqgrid(GetExerciseInfoListForJqgridParam param)
        {
            try
            {
                //int totalCount = 0;

                //IExerciseCtrl exerciseCtrl = new ExerciseCtrl();
                //List<Biz_Exercise> exerciseList = exerciseCtrl.GetExerciseInfoListForJqgrid(param.ExerciseName, param.rows, param.page, ref totalCount);
                //List<GetExerciseInfoListForJqgridResult> getExerciseInfoListForJqgridResult = exerciseList
                //    .Select(o => new GetExerciseInfoListForJqgridResult()
                //    {
                //        ExerciseId = o.Id,
                //        ExerciseName = o.ExerciseName,
                //        PaperCode = o.PaperCode,
                //        IsRandom = o.IsRandom ? "是" : "否",
                //        IsShowRightAnswer = o.IsShowRightAnswer ? "是" : "否",
                //        IsLimitTime = o.IsLimitTime ? "是" : "否",
                //        LimitTime = o.LimitTime,
                //        IsSaveAnswer = o.IsSaveAnswer ? "是" : "否"
                //    }).ToList();
                //JqGridData jqgResult = ConvertIListToJqGridData(param.page, param.rows, totalCount, getExerciseInfoListForJqgridResult);
                //return Json(jqgResult, JsonRequestBehavior.AllowGet);
                return default;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public class GetExerciseInfoListForJqgridResult
        {
            public int ExerciseId { get; set; }
            public string ExerciseName { get; set; }
            public string PaperCode { get; set; }
            public string IsRandom { get; set; }
            //是否顺序出题
            public string IsOrder { get; set; }
            public string IsShowRightAnswer { get; set; }
            //是否限制时长
            public string IsLimitTime { get; set; }
            //限制时长(单位分钟)
            public int LimitTime { get; set; }
            public string IsSaveAnswer { get; set; }
        }
        #endregion

        #region 新增练习管理
        public class InsertExerciseManageParam
        {
            public string ExerciseName { get; set; }
            public string PaperCode { get; set; }
            public string IsRandom { get; set; }
            public string IsShowRightAnswer { get; set; }
            //是否限制时长
            public string IsLimitTime { get; set; }
            //限制时长(单位分钟)
            public int LimitTime { get; set; }
            public string IsSaveAnswer { get; set; }
        }
        [JsonException]
        public bool InsertExerciseManager(InsertExerciseManageParam param)
        {
            try
            {
                //UserInfo userInfo = LoginUserInfo;

                //IExerciseCtrl exerciseCtrl = new ExerciseCtrl();
                //Biz_Exercise exercise = new Biz_Exercise()
                //{
                //    ExerciseName = param.ExerciseName,
                //    PaperCode = param.PaperCode,
                //    IsRandom = param.IsRandom == "是" ? true : false,
                //    IsShowRightAnswer = param.IsShowRightAnswer == "是" ? true : false,
                //    IsLimitTime = param.IsLimitTime == "是" ? true : false,
                //    LimitTime = param.LimitTime,
                //    IsSaveAnswer = param.IsSaveAnswer == "是" ? true : false,
                //    CreateBy = userInfo.UserId,
                //    CreateDate = DateTime.Now
                //};
                //exerciseCtrl.InsertExerciseManage(exercise);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        #endregion

        #region  根据exerciseId获取练习管理信息

        public class GetExerciseManageListDataByExerciseIdParam
        {
            public int ExerciseId { get; set; }
        }

        [JsonException]
        public JsonResult GetExerciseManageListDataByExerciseId(GetExerciseManageListDataByExerciseIdParam param)
        {
            try
            {

                //IExerciseCtrl exerciseCtrl = new ExerciseCtrl();
                //Biz_Exercise exercise = exerciseCtrl.GetExerciseManageListDataByExerciseId(param.ExerciseId);
                //GetExerciseManageListDataByExerciseIdResult getExerciseManageListDataByExerciseIdResult = new GetExerciseManageListDataByExerciseIdResult
                //{
                //    ExerciseName = exercise.ExerciseName,
                //    PaperCode = exercise.PaperCode,
                //    IsRandom = exercise.IsRandom ? "是" : "否",
                //    IsShowRightAnswer = exercise.IsShowRightAnswer ? "是" : "否",
                //    IsLimitTime = exercise.IsLimitTime ? "是" : "否",
                //    LimitTime = exercise.LimitTime,
                //    IsSaveAnswer = exercise.IsSaveAnswer ? "是" : "否"
                //};
                //return Json(getExerciseManageListDataByExerciseIdResult, JsonRequestBehavior.AllowGet);
                return default;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class GetExerciseManageListDataByExerciseIdResult
        {
            public string ExerciseName { get; set; }
            public string PaperCode { get; set; }
            public string IsRandom { get; set; }
            public string IsShowRightAnswer { get; set; }
            //是否限制时长
            public string IsLimitTime { get; set; }
            //限制时长(单位分钟)
            public int LimitTime { get; set; }
            public string IsSaveAnswer { get; set; }
        }
        #endregion

        #region  修改练习管理
        public class UpdateExerciseManageParam
        {
            public int ExerciseId { get; set; }
            public string ExerciseName { get; set; }
            public string PaperCode { get; set; }
            public string IsRandom { get; set; }
            public string IsShowRightAnswer { get; set; }
            //是否限制时长
            public string IsLimitTime { get; set; }
            //限制时长(单位分钟)
            public int LimitTime { get; set; }
            public string IsSaveAnswer { get; set; }
        }

        [JsonException]
        public bool UpdateExerciseManager(UpdateExerciseManageParam param)
        {
            try
            {

                //IExerciseCtrl exerciseCtrl = new ExerciseCtrl();
                //Biz_Exercise exercise = new Biz_Exercise()
                //{
                //    Id = param.ExerciseId,
                //    ExerciseName = param.ExerciseName,
                //    PaperCode = param.PaperCode,
                //    IsRandom = param.IsRandom == "是" ? true : false,
                //    IsShowRightAnswer = param.IsShowRightAnswer == "是" ? true : false,
                //    IsLimitTime = param.IsLimitTime == "是" ? true : false,
                //    LimitTime = param.LimitTime,
                //    IsSaveAnswer = param.IsSaveAnswer == "是" ? true : false
                //};
                //exerciseCtrl.UpdateExerciseManage(exercise);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        #endregion

        #region  删除练习管理

        public class DeleteExerciseManagerParam
        {
            public string ExerciseManageIdList { get; set; }
        }

        [JsonException]
        public bool DeleteExerciseManager(DeleteExerciseManagerParam param)
        {
            bool deleteResult = false;
            //TransactionScope scope = CreateTransaction();
            //try
            //{
            //    if (!param.ExerciseManageIdList.IsNull())
            //    {
            //        List<int> currentExerciseManageIdList = param.ExerciseManageIdList.Split(',').Where(x => !x.IsNull()).Select(x => Convert.ToInt32(x)).ToList();

            //        IExerciseCtrl exerciseCtrl = new ExerciseCtrl();
            //        exerciseCtrl.DeleteExerciseManage(currentExerciseManageIdList);
            //        deleteResult = true;
            //    }
            //    TransactionCommit(scope);
            //}
            //catch (Exception ex)
            //{
            //    TransactionRollback(scope);
            //    throw ex;
            //}
            return deleteResult;
        }
        #endregion

    }
}