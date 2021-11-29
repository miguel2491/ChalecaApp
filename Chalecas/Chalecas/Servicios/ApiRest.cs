using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chalecas.Models;
using System;

namespace Chalecas.Servicios
{
    public class ApiRest
    {
        private string URL = "https://wenpanic.000webhostapp.com/api/";

        public async Task<HttpResponseMessage> GetUser()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(URL + "getUsuarios");
            return response;
        }

        public async Task<HttpResponseMessage> GetLogin(string username, string password, string token)
        {
            var client = new HttpClient();
            int lengToken = token.Length;
            if (lengToken == 0)
                token = "NA";
            var model = new Usuario
            {
                username = username,
                password = password,
                token = token
            };

            var json = JsonConvert.SerializeObject(model);
            StringContent body = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(URL + "login", body);
            return response;
        }

        public async Task<HttpResponseMessage> SetPos(string iduser, string lat, string lon)
        {
            var client = new HttpClient();
            var model = new LocalizaModel
            {
                id_usuario = iduser,
                latitud = lat,
                longitud = lon,
                status = "Activo"
            };
            var json = JsonConvert.SerializeObject(model);
            StringContent body = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(URL + "ubicame", body);
            return response;
        }

        public async Task<HttpResponseMessage> GetPosUser(string idu)
        {
            var client = new HttpClient();
            var model = new Usuario
            {
                nombre = idu
            };
            var json = JsonConvert.SerializeObject(model);
            StringContent body = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(URL + "ubicala", body);
            return response;
        }

        public async Task<HttpResponseMessage> SendLlamada(string id_emisor, string usuario, string mensaje)
        {
            var client = new HttpClient();
            var model = new NotificacionM
            {
                id_user_emisor = id_emisor,
                id_user_receptor = usuario,
                mensaje = mensaje
            };
            var json = JsonConvert.SerializeObject(model);
            StringContent body = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(URL + "sendLlamada", body);
            return response;
        }

        public async Task<HttpResponseMessage> SendLlamadaSindica(string id_emisor)
        {
            var client = new HttpClient();
            string id_emisor2 = Convert.ToString(id_emisor);
            var model = new NotificacionM
            {
                id_user_emisor = id_emisor2
            };
            var json = JsonConvert.SerializeObject(model);
            StringContent body = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(URL + "SendLlamadaSindico", body);
            return response;
        }

        public async Task<HttpResponseMessage> GetChalecas()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(URL + "getChaleca");
            return response;
        }
        //-----------------------------------------------------------------------------
    }
}
