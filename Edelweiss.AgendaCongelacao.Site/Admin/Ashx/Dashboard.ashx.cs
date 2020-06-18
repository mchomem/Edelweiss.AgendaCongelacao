using Edelweiss.AgendaCongelacao.Model.Dashboard;
using Edelweiss.AgendaCongelacao.Model.Json;
using Edelweiss.AgendaCongelacao.Model.Repositories;
using Edelweiss.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Edelweiss.AgendaCongelacao.Site.Admin.Ashx
{
    /// <summary>
    /// Summary description for Dashboard
    /// </summary>
    public class Dashboard : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            String op = context.Request["op"];

            switch (op)
            {
                case "consultarAgendasAno":
                    this.ConsultarAgendasAno(context);
                    break;

                case "consultarAnosDisponiveis":
                    this.ConsultarAnosDisponiveis(context);
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

        private void ConsultarAgendasAno(HttpContext context)
        {
            AgendasAno agendasAno = new AgendasAno();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ReturnJSON<AgendasAno> returnJSON = new ReturnJSON<AgendasAno>();
            String retorno = null;

            try
            {
                String json = new StreamReader(context.Request.InputStream).ReadToEnd();
                agendasAno = (AgendasAno)serializer.Deserialize<AgendasAno>(json);
                agendasAno = new AgendasAnoRepository().ObterAgendasAno(new AgendasAno() { Ano = agendasAno.Ano });

                returnJSON.Entity = agendasAno;
                returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.SUCCESS);
                context.Response.StatusCode = 200;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                returnJSON.Message = "Não foi possível recuperar os registros.";
                returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.ERROR);
                context.Response.StatusCode = 500;
            }

            retorno = serializer.Serialize(returnJSON);
            context.Response.ContentType = "text/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(retorno);
        }

        private void ConsultarAnosDisponiveis(HttpContext context)
        {
            AgendasAno agendasAno = new AgendasAno();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ReturnJSON<List<Int32>> returnJSON = new ReturnJSON<List<Int32>>();
            String retorno = null;

            try
            {
                List<Int32> anosDisponiveis = new AgendasAnoRepository().AnosDisponiveis();
                returnJSON.Entity = anosDisponiveis;
                returnJSON.ReturnCode = Enum.GetName(typeof(ReturnType), ReturnType.SUCCESS);
                context.Response.StatusCode = 200;
            }
            catch (Exception e)
            {
                Log.Create(e);
                Email.Send("Agendamento de congelação - falha na aplicação", e);
                returnJSON.Message = "Não foi possível recuperar os registros.";
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