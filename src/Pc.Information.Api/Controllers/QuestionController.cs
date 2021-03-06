﻿using Microsoft.AspNetCore.Mvc;
using Pc.Information.Interface.IQuestionBll;
using Pc.Information.Model.QuestionInfo;
using System;
using Pc.Information.Model.BaseModel;
using System.Collections.Generic;
using System.Linq;
using Pc.Information.Interface.IQuestionReplyBll;
using FreshCommonUtility.DataConvert;

namespace Pc.Information.Api.Controllers
{
    /// <summary>
    /// Question controller
    /// </summary>
    public class QuestionController : BaseController
    {
        /// <summary>
        /// Interface question info server.
        /// </summary>
        private IQuestionBll QuestionInfoBll { get; set; }

        /// <summary>
        /// Interface question reply info server.
        /// </summary>
        private IQuestionReplyBll QuestionReplyBll { get; set; }

        /// <summary>
        /// Constructed function.
        /// </summary>
        /// <param name="questionInfoBll">questionInfoBll</param>
        /// <param name="questionReplyBll">questionReplyBll</param>
        public QuestionController(IQuestionBll questionInfoBll, IQuestionReplyBll questionReplyBll)
        {
            QuestionInfoBll = questionInfoBll;
            QuestionReplyBll = questionReplyBll;
        }

        /// <summary>
        /// Add new question info.
        /// </summary>
        /// <param name="newQuestionInfo"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [Route("AddQuestionInfo")]
        public ApiResultModel<DataBaseModel> AddQuestionInfo(PiFQuestionInfoModel newQuestionInfo)
        {
            var baseDataModel = new DataBaseModel();
            if (string.IsNullOrEmpty(newQuestionInfo.PiFQuestionTitle) || string.IsNullOrEmpty(newQuestionInfo.PiFQuestionContent))
            {
                baseDataModel.StateCode = "1000";
                baseDataModel.StateDesc = "Request data is invalid.";
                return ResponseDataApi(baseDataModel);
            }
            newQuestionInfo.PiFCreateTime = DateTime.Now;
            var info = QuestionInfoBll.AddQuestionInfo(newQuestionInfo);
            if (info < 1)
            {
                baseDataModel.StateCode = "1001";
                baseDataModel.StateDesc = "Add faild.";
            }
            else
            {
                baseDataModel.StateCode = "0000";
                baseDataModel.StateDesc = "Add Success.";
            }
            return ResponseDataApi(baseDataModel);
        }

        /// <summary>
        /// Add new question view info.
        /// </summary>
        /// <param name="questionId">questionId</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [Route("AddQuestionViewCount")]
        public ApiResultModel<DataBaseModel> AddQuestionView(int questionId, int userId)
        {
            var baseDataModel = new DataBaseModel();
            if (questionId < 1)
            {
                baseDataModel.StateCode = "1000";
                baseDataModel.StateDesc = "Request data is invalid.";
                return ResponseDataApi(baseDataModel);
            }
            var info = QuestionInfoBll.AddQuestionViewInfo(questionId, userId);
            if (info < 1)
            {
                baseDataModel.StateCode = "1001";
                baseDataModel.StateDesc = "Add faild.";
            }
            else
            {
                baseDataModel.StateCode = "0000";
                baseDataModel.StateDesc = "Add Success.";
            }
            return ResponseDataApi(baseDataModel);
        }

        /// <summary>
        /// Updata question info.
        /// </summary>
        /// <param name="newQuestionInfo"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [Route("UpdateQuestionInfo")]
        public ApiResultModel<DataBaseModel> UpdateQuestionInfo(PiFQuestionInfoModel newQuestionInfo)
        {
            var baseDataModel = new DataBaseModel();
            if (string.IsNullOrEmpty(newQuestionInfo.PiFQuestionTitle) || string.IsNullOrEmpty(newQuestionInfo.PiFQuestionContent))
            {
                baseDataModel.StateCode = "1000";
                baseDataModel.StateDesc = "Request data is invalid.";
                return ResponseDataApi(baseDataModel);
            }
            var info = QuestionInfoBll.UpdateQuestionInfo(newQuestionInfo);
            if (info < 1)
            {
                baseDataModel.StateCode = "1001";
                baseDataModel.StateDesc = "Update faild.";
            }
            else
            {
                baseDataModel.StateCode = "0000";
                baseDataModel.StateDesc = "Update Success.";
            }
            return ResponseDataApi(baseDataModel);
        }

        /// <summary>
        /// search Qustion info.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="startTime">start time</param>
        /// <param name="endTime">end time</param>
        /// <param name="title">Fuzzy search word</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">pagesize</param>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        [Route("SearchQustionInfo")]
        public ApiResultModel<List<PiFQuestionInfoWithReplyModel>> SearchQustionInfo(long id = 0, string startTime = "1900-1-1", string endTime = "1900-1-1", string title = null, int pageIndex = 1, int pageSize = 10, int userId = 0)
        {
            var tStartTime = DataTypeConvertHelper.ToDateTime(startTime);
            var tTendTime = DataTypeConvertHelper.ToDateTime(endTime);
            var dataList = QuestionInfoBll.SearchQustionInfo(id, tStartTime, tTendTime, title, pageIndex, pageSize);
            var resulteQuestion = new List<PiFQuestionInfoWithReplyModel>();
            if (dataList != null && dataList.Any())
            {
                dataList.ForEach(f =>
                {
                    var itemModel = new PiFQuestionInfoWithReplyModel { Id = f.Id, PiFCreateTime = f.PiFCreateTime, PiFQuestionContent = f.PiFQuestionContent, PiFQuestionTitle = f.PiFQuestionTitle, PiFSendUserId = f.PiFSendUserId, PiFSendUserName = f.PiFSendUserName, ViewCount = f.ViewCount ?? 0 };
                    long countNubmer;
                    itemModel.ReplyInfoList = QuestionReplyBll.GetReplyInfoList(out countNubmer, f.Id, userId: userId);
                    itemModel.CountNumber = countNubmer;
                    resulteQuestion.Add(itemModel);
                });
            }
            return ResponseDataApi(resulteQuestion);
        }

        /// <summary>
        /// GetHotReplyQuestionInfo
        /// </summary>
        /// <returns></returns>
        [Route("GetHotReplyQuestionInfo")]
        [HttpGet]
        [HttpPost]
        public ApiResultModel<List<PiFQuestionInfoWithReplyModel>> GetHotReplyQuestionInfo()
        {
            var resulteList = QuestionInfoBll.GetHotReplyQuestionInfo();
            return ResponseDataApi(resulteList);
        }

        /// <summary>
        /// GetHotReplyQuestionInfo
        /// </summary>
        /// <returns></returns>
        [Route("GetHotViewQuestionInfo")]
        [HttpGet]
        [HttpPost]
        public ApiResultModel<List<PiFQuestionInfoWithReplyModel>> GetHotViewQuestionInfo()
        {
            var resulteList = QuestionInfoBll.GetHotViewQuestionInfo();
            return ResponseDataApi(resulteList);
        }
    }
}
