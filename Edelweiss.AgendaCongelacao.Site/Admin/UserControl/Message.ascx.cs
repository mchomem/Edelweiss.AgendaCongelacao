using System;

namespace Edelweiss.AgendaCongelacao.Site.Admin.UserControl
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

        private void Hide()
        {
            this.pnlMessage.Attributes["style"] = "display:none;";
        }

        public void Show(String title, String text, Type type)
        {
            String defaultCssClass = "w3-container";
            String defaultButtonClass = "w3-btn";

            switch (type)
            {
                case Type.Error:
                    this.modalHeader.Attributes["class"] = String.Format("{0} {1}", defaultCssClass, "w3-red");
                    this.btnClose.CssClass = String.Format("{0} {1}", defaultButtonClass, "w3-red");
                    break;

                case Type.Information:
                    this.modalHeader.Attributes["class"] = String.Format("{0} {1}", defaultCssClass, "w3-blue");
                    this.btnClose.CssClass = String.Format("{0} {1}", defaultButtonClass, "w3-blue");
                    break;

                case Type.Success:
                    this.modalHeader.Attributes["class"] = String.Format("{0} {1}", defaultCssClass, "w3-green");
                    this.btnClose.CssClass = String.Format("{0} {1}", defaultButtonClass, "w3-green");
                    break;

                case Type.Warning:
                    this.modalHeader.Attributes["class"] = String.Format("{0} {1}", defaultCssClass, "w3-yellow");
                    this.btnClose.CssClass = String.Format("{0} {1}", defaultButtonClass, "w3-yellow");
                    break;

                default:
                    this.modalHeader.Attributes["class"] = String.Format("{0} {1}", defaultCssClass, "w3-blue");
                    this.btnClose.CssClass = String.Format("{0} {1}", defaultButtonClass, "w3-blue");
                    break;
            }

            this.pnlMessage.Attributes["style"] = "display:block";
            this.litTitle.Text = title;
            this.litMenssage.Text = text;
            this.btnClose.Focus();
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