using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SPA_Datahandler.Datamodel
{
    [DataContract]
    public class OrderItemReport_
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public DateTime ReportDate { get; set; }
        [DataMember]
        public int OrderItemId { get; set; }
        [DataMember]
        public List<OrderItemReportAppendix> Appendix { get; set; }

        public string Visibility { get; set; }

        private string shortComment;

        public string ShortComment
        {
            get
            {
                return shortComment;
            }
            set
            {

                if (Comment.Contains(Environment.NewLine))
                {
                    shortComment = String.Format("{0} {1}", Comment.Substring(0, Comment.IndexOf(Environment.NewLine)), "(...)");
                }
                else if (Comment.Length > 50)
                {
                    shortComment = String.Format("{0} {1}", Comment.Substring(0, 50), "(...)");
                }
                else
                {
                    shortComment = this.Comment;
                }

            }
        }

        public OrderItemReport_()
        {
            Visibility = "Collapsed";
        }
    }
}
