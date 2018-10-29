using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSurveying2017.Model.Model;

namespace WebSurveying2017.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string UpdatedText { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastEdited { get; set; }

     //   public State CommentState { get; set; }

        public int? ParentId { get; set; }
        public CommentViewModel ParentComment { get; set; }

        public int UserId { get; set; }
        public UserViewModel User { get; set; }

        public List<CommentViewModel> SubComments { get; set; }

        public SurveyViewModel Survey { get; set; }

        public LikeOrDislikeComment LoggedUserLikeOrDislike { get; set; }

        public int LikeCount { get; set; }

        public int DislikeCount { get; set; }

        public int SubCommentsCount { get; set; }

        public int SurveyId { get; set; }

    }
}