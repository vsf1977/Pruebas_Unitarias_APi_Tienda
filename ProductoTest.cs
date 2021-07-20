using API_Tienda.Controllers;
using API_Tienda.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace API_Tiendas.Test
{
    [TestClass]
    public class ProductoTest
    {
        [TestMethod]
        public void Validar_Si_Crea_Producto()
        {
            // Arrange
            Producto producto = new Producto();
            ProductoController productoController = new ProductoController();
            producto.idProducto = "205";
            producto.nombre = "Camisa";
            producto.descripcion = "Camisa fina";
            producto.precio = 500;
            producto.descuento = 10;
            producto.pais = "Colombia";
            //Act
            var response = productoController.Post(producto);
            //Assert 
            //Se verifica que el tipo de resultado sea ObjectResult
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
            Assert.IsInstanceOfType((response as ObjectResult).Value, typeof(string));

        }

        [TestMethod]
        public void No_debe_crear_producto_porque_el_descuento_es_mayor_a_50_en_Colombia()
        {
            // Arrange
            Producto producto = new Producto();
            ProductoController productoController = new ProductoController();
            producto.idProducto = "205";
            producto.nombre = "Camisa";
            producto.descripcion = "Camisa fina";
            producto.precio = 500;
            producto.descuento = 51;
            producto.pais = "Colombia";
            //Act
            var response = productoController.Post(producto);
            //Assert
            //Se verifica que el tipo de resultado sea BadRequestObjectResult con un valor string y se verifica que el string sea el esperado
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult));
            Assert.IsInstanceOfType((response as BadRequestObjectResult).Value, typeof(string));
            Assert.AreEqual((response as BadRequestObjectResult).Value, "The discount musn´t be greater than 50%");
        }


        [TestMethod]
        public void Debe_mostrar_la_consulta_con_un_parametro()
        {
            // Arrange
            ProductoController productoController = new ProductoController();
            //Act
            var response = productoController.Get("jean", null, null, null, null);
            //Assert
            //Se verifica que el tipo de resultado sea OkObjectResult con un valor de lista de producto y una cantidad de 2
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.IsInstanceOfType(((OkObjectResult)response).Value, typeof(List<Producto>));
            List<Producto> results = (List<Producto>)((OkObjectResult)response).Value;
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void Debe_mostrar_la_consulta_con_tres_parametro()
        {
            // Arrange
            ProductoController productoController = new ProductoController();
            //Act
            var response = productoController.Get(null, 250, 100, null, "colombia");
            //Assert
            //Se verifica que el tipo de resultado sea OkObjectResult con un valor de lista de producto y una cantidad de 4
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.IsInstanceOfType(((OkObjectResult)response).Value, typeof(List<Producto>));
            List<Producto> results = (List<Producto>)((OkObjectResult)response).Value;
            Assert.AreEqual(4, results.Count);
        }

        [TestMethod]
        public void Debe_mostrar_error_en_la_consulta_porque_falta_un_parametro_para_el_precio_o_el_preciomax_es_menor_que_preciomin()
        {
            // Arrange
            ProductoController productoController = new ProductoController();
            //Act
            var response = productoController.Get(null, 6, 7, null, "colombia");
            //Assert
            //Se verifica que el tipo de resultado sea BadRequestObjectResult porque falta un parametro de precio.
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult));
            Assert.IsInstanceOfType((response as BadRequestObjectResult).Value, typeof(string));
            Assert.AreEqual((response as BadRequestObjectResult).Value, "Parameter Error: The max price and the the min price must be valid and the max price must be greater than the min price");
        }
    }
}
