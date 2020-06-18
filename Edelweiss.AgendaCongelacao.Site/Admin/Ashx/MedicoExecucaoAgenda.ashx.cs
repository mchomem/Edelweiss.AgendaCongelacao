using Edelweiss.AgendaCongelacao.Model;
using Edelweiss.AgendaCongelacao.Model.Json;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.Utils;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Edelweiss.AgendaCongelacao.Site.Admin.Ashx
{
    /// <summary>
    /// Summary description for MedicoExecucaoAgenda
    /// </summary>
    public class MedicoExecucaoAgenda : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            String op = context.Request["op"];

            switch (op)
            {

                case "salvar":
                    this.Salvar(context);
                    break;
                // More options of CRUD inside "case" clausules!

                default:
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private void Salvar(HttpContext context)
        {
            Model.Entities.MedicoExecucaoAgenda medicoExecucaoAgenda = new Model.Entities.MedicoExecucaoAgenda();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ReturnJSON<Model.Entities.MedicoExecucaoAgenda> returnJSON = new ReturnJSON<Model.Entities.MedicoExecucaoAgenda>();
            String retorno = null;

            try
            {
                String json = new StreamReader(context.Request.InputStream).ReadToEnd();
                medicoExecucaoAgenda = (Model.Entities.MedicoExecucaoAgenda)serializer.Deserialize<Model.Entities.MedicoExecucaoAgenda>(json);

                // Registro em edição?
                if (medicoExecucaoAgenda.MedicoExecucaoAgendaID.HasValue && medicoExecucaoAgenda.MedicoExecucaoAgendaID > 0)
                {
                    new MedicoExecucaoAgendaRepository().Update(medicoExecucaoAgenda);
                }
                else
                {
                    new MedicoExecucaoAgendaRepository().Create(medicoExecucaoAgenda);
                }

                returnJSON.Message = "Registro salvo com sucesso.";
                returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.SUCCESS);
                context.Response.StatusCode = 201;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                returnJSON.Message = "Não foi possível salvar o registro.";
                returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.ERROR);
                context.Response.StatusCode = 500;
            }

            retorno = serializer.Serialize(returnJSON);
            context.Response.ContentType = "text/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(retorno);
        }
    }
}