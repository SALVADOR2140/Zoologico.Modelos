using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Zoologico.Modelos;              // acceso a ApiResult y Especie

namespace Zoologico.ApiTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var httpClient = new HttpClient();
            var rutaEspecies = "api/Especies";

            httpClient.BaseAddress = new Uri("https://localhost:7014/");

            // LECTURA DE DATOS
            var response = httpClient.GetAsync(rutaEspecies).Result;
            var json = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine("JSON crudo de la API (GET):");
            Console.WriteLine(json);

            var especies = JsonConvert.DeserializeObject<ApiResult<List<Especie>>>(json);

            if (especies == null)
            {
                Console.WriteLine("No se pudo deserializar a ApiResult<List<Especie>>.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("=== LECTURA INICIAL ===");
            Console.WriteLine("Success: " + especies.Success);
            Console.WriteLine("Message: " + especies.Message);
            Console.WriteLine("Total especies: " + (especies.Data?.Count ?? 0));

            // INSERCIÓN DE DATOS
            var nuevaEspecie = new Especie
            {
                Codigo = 0,
                NombreComun = "XYZ"
            };

            var especieJson = JsonConvert.SerializeObject(nuevaEspecie);
            var content = new StringContent(especieJson, Encoding.UTF8, "application/json");

            response = httpClient.PostAsync(rutaEspecies, content).Result;
            json = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine("\nJSON crudo del POST:");
            Console.WriteLine(json);

            var especieCreada = JsonConvert.DeserializeObject<ApiResult<Especie>>(json);

            Console.WriteLine("\n=== INSERCIÓN ===");
            Console.WriteLine("Success: " + especieCreada.Success);
            Console.WriteLine("Message: " + especieCreada.Message);
            Console.WriteLine("Codigo creado: " + especieCreada.Data?.Codigo);

            Console.WriteLine("\nFIN (por ahora). Pulsa ENTER...");
            Console.ReadLine();

            // si no se creó bien, no sigas con actualización
            if (!especieCreada.Success || especieCreada.Data == null)
            {
                Console.WriteLine("\nNo se pudo crear la especie, se detiene la prueba.");
                Console.ReadLine();
                return;
            }

            // ACTUALIZACIÓN DE DATOS
            especieCreada.Data.NombreComun = "XYZ Actualizado";

            especieJson = JsonConvert.SerializeObject(especieCreada.Data);
            content = new StringContent(especieJson, Encoding.UTF8, "application/json");

            response = httpClient.PutAsync(
                $"{rutaEspecies}/{especieCreada.Data.Codigo}", content).Result;

            json = response.Content.ReadAsStringAsync().Result;

            var especieActualizada = JsonConvert.DeserializeObject<ApiResult<Especie>>(json);

            Console.WriteLine("\n=== ACTUALIZACIÓN ===");
            Console.WriteLine("Success: " + especieActualizada.Success);
            Console.WriteLine("Message: " + especieActualizada.Message);
            Console.WriteLine("NombreComun: " + especieActualizada.Data?.NombreComun);

            Console.WriteLine("\nFIN DE PRUEBA. Pulsa ENTER...");

            ////// ELIMINACIÓN DE DATOS
            ////int codigoEliminar = 5; // aquí pones el Código que quieras borrar

            ////response = httpClient.DeleteAsync(
            ////    $"{rutaEspecies}/{codigoEliminar}").Result;

            ////json = response.Content.ReadAsStringAsync().Result;

            ////Console.WriteLine("\nJSON crudo del DELETE:");
            ////Console.WriteLine(json);

            ////var especieEliminada = JsonConvert.DeserializeObject<ApiResult<Especie>>(json);

            ////Console.WriteLine("\n=== ELIMINACIÓN ===");
            ////Console.WriteLine("Success: " + especieEliminada.Success);
            ////Console.WriteLine("Message: " + especieEliminada.Message);
            ////Console.WriteLine("Codigo eliminado: " + especieEliminada.Data?.Codigo);

            ////Console.WriteLine("\nFIN. Pulsa ENTER...");
            ////Console.ReadLine();


            Console.ReadLine();
        }
    }
}
