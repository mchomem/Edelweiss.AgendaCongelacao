using System;

namespace Edelweiss.AgendaCongelacao.Site.UserControl
{
    public partial class Message : System.Web.UI.UserControl
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            ;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        #endregion

        #region Methods

        public void Show(String title, String text, Type type)
        {
            String defaultCssClass = "modal-header text-white py-2";
            String defaultButtonClass = "btn btn-sm text-white";

            switch (type)
            {
                case Type.Error:
                    this.modalHeader.Attributes["class"] = String.Format("{0} {1}", defaultCssClass, "bg-danger");
                    this.btnClose.CssClass = String.Format("{0} {1}", defaultButtonClass, "btn-danger");
                    break;

                case Type.Information:
                    this.modalHeader.Attributes["class"] = String.Format("{0} {1}", defaultCssClass, "bg-primary");
                    this.btnClose.CssClass = String.Format("{0} {1}", defaultButtonClass, "btn-primary");
                    break;

                case Type.Success:
                    this.modalHeader.Attributes["class"] = String.Format("{0} {1}", defaultCssClass, "bg-success");
                    this.btnClose.CssClass = String.Format("{0} {1}", defaultButtonClass, "btn-success");
                    break;

                case Type.Warning:
                    this.modalHeader.Attributes["class"] = String.Format("{0} {1}", defaultCssClass, "bg-warning");
                    this.btnClose.CssClass = String.Format("{0} {1}", defaultButtonClass, "btn-warning");
                    break;

                default:
                    this.modalHeader.Attributes["class"] = String.Format("{0} {1}", defaultCssClass, "bg-primary");
                    this.btnClose.CssClass = String.Format("{0} {1}", defaultButtonClass, "btn-primary");
                    break;
            }

            this.pnlMessage.CssClass = "modal fade show";
            this.pnlMessage.Attributes["style"] = "display:block;";
            this.litTitle.Text = title;
            this.litMenssage.Text = text;
        }

        public void Hide()
        {
            this.pnlMessage.CssClass = "modal fade";
            this.pnlMessage.Attributes["style"] = "display:none;";
        }

        #endregion

        #region Enuns

        public enum Type
        {
            Error
            , Information
            , Success
            , Warning
        }

        #endregion
    }
}
