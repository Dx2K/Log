using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG
{
    /// <summary>
    /// <para>
    ///  Permite el guardado de LOG en la Base de Datos.
    ///  Se necesitan 3 tablas en la BD y el string de conexión al la BD dentro del web(app).config del proyecto en el que se use:
    ///  El string de conexión debe de llamarse LOGSISTEMAS
    /// </para>
    /// <para>
    /// La tabla Sistemas contiene los sistemas de los cualse se guardará log
    /// </para>
    /// <para>
    /// CREATE TABLE [dbo].[Sistema](
    /// [Id] [int] IDENTITY(1,1) NOT NULL,
    /// [Glosa] [nvarchar](250) NOT NULL,
    /// [Sigla] [nchar](10) NOT NULL,
    /// CONSTRAINT [PK_Sistema] PRIMARY KEY CLUSTERED 
    /// (
    /// [Id] ASC
    /// )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    /// ) ON [PRIMARY]
    /// </para>
    /// <para>
    /// La tabla Tipo contiene los tipos de log a guardar (info, error, debug, warning). El valor de cada tipo debe ser igual al enumerador Tipo de la solución.
    /// </para>
    /// <para>
    /// CREATE TABLE [dbo].[Tipo](
    /// [Id] [int] NOT NULL,
    /// [Glosa] [nchar](50) NOT NULL,
    /// CONSTRAINT [PK_Tipo] PRIMARY KEY CLUSTERED 
    /// (
    /// [Id] ASC
    /// )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    /// ) ON [PRIMARY]
    /// </para>
    /// <para>
    /// La tabla Log guarda el log del sistema, esta referencia a las tablas anteriores.
    /// </para>
    /// <para>
    /// CREATE TABLE [dbo].[Log](
    /// [Id] [int] IDENTITY(1,1) NOT NULL, --Identificador único del registro
    /// [Sistema] [int] NOT NULL, --Identificador del sistema del cual se guarda el log
    /// [Tipo] [int] NOT NULL, --Tipo de log guardado (error, info, debug, warning
    /// [Fecha] [datetime] NOT NULL, --Fecha de la ocurrencia
    /// [Excepcion] [nvarchar](max) NULL, --Stacktrace del error
    /// [Mensaje] [nvarchar](max) NULL, --Mensaje de error
    /// CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
    /// (
    /// [Id] ASC
    /// )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    /// ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    /// GO
    /// 
    /// ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_Sistema] FOREIGN KEY([Sistema])
    /// REFERENCES [dbo].[Sistema] ([Id])
    /// GO
    /// 
    /// ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_Sistema]
    /// GO
    /// 
    /// ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_Tipo] FOREIGN KEY([Tipo])
    /// REFERENCES [dbo].[Tipo] ([Id])
    /// GO
    /// 
    /// ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_Tipo]
    /// GO
    /// </para>
    /// </summary>
    public class Logger
    {
        private enum Tipos
        {
            Error = 1,
            Info = 2,
            Debug = 3,
            Warning = 4
        }

        /// <summary>
        ///  Guarda registro de LOG de error
        ///  <para>
        ///  <code>Ejemplo. ERROR(1, e, "Mensaje de error")</code>
        ///  </para>
        /// </summary>
        /// <example>Ejemplo. ERROR(1, e, "Mensaje de error")</example>
        /// <param name="Sistema">Int. Identificador del sistema</param>
        /// <param name="Excepcion">Exception. Excepción arrojada por el sistema</param>
        /// <param name="Mensaje">String. Mensaje para humanos</param>
        /// <returns>Retorna Id del registro, si el guardado falla se retorna -1</returns>
        public int ERROR(int Sistema, Exception Excepcion, string Mensaje)
        {
            int Tipo = (int)Tipos.Error;
            return this.Guardar(Tipo, Sistema, Excepcion, Mensaje);
        }

        /// <summary>
        ///  Guarda registro de LOG de error
        ///  <para>
        ///  <code>Ejemplo. ERROR(1, "Mensaje de error")</code>
        ///  </para>
        /// </summary>
        /// <example>Ejemplo. ERROR(1, "Mensaje de error")</example>
        /// <param name="Sistema">Int. Identificador del sistema</param>
        /// <param name="Mensaje">String. Mensaje para humanos</param>
        /// <returns>Retorna Id del registro, si el guardado falla se retorna -1</returns>
        public int ERROR(int Sistema, string Mensaje)
        {
            int Tipo = (int)Tipos.Error;
            return this.Guardar(Tipo, Sistema, Mensaje);
        }

        /// <summary>
        ///  Guarda registro de LOG de información
        ///  <para>
        ///  <code>Ejemplo. INFO(1, e, "Mensaje de error")</code>
        ///  </para>
        /// </summary>
        /// <example>Ejemplo. INFO(IdentificadorSistema, Excepcion, Mensaje)</example>
        /// <param name="Sistema">Int. Identificador del sistema</param>
        /// <param name="Excepcion">Exception. Excepción arrojada por el sistema</param>
        /// <param name="Mensaje">String. Mensaje para humanos</param>
        /// <returns>Retorna Id del registro, si el guardado falla se retorna -1</returns>
        public int INFO(int Sistema, Exception Excepcion, string Mensaje)
        {
            int Tipo = (int)Tipos.Info;
            return this.Guardar(Tipo, Sistema, Excepcion, Mensaje);
        }

        /// <summary>
        ///  Guarda registro de LOG de información
        ///  <para>
        ///  <code>Ejemplo. INFO(1, "Mensaje de error")</code>
        ///  </para>
        /// </summary>
        /// <example>Ejemplo. INFO(IdentificadorSistema, Mensaje)</example>
        /// <param name="Sistema">Int. Identificador del sistema</param>
        /// <param name="Mensaje">String. Mensaje para humanos</param>
        /// <returns>Retorna Id del registro, si el guardado falla se retorna -1</returns>
        public int INFO(int Sistema, string Mensaje)
        {
            int Tipo = (int)Tipos.Info;
            return this.Guardar(Tipo, Sistema, Mensaje);
        }

        /// <summary>
        ///  Guarda registro de LOG de advertencia
        ///  <para>
        ///  <code>Ejemplo. WARNING(1, e, "Mensaje de error")</code>
        ///  </para>
        /// </summary>
        /// <example>Ejemplo. WARNING(IdentificadorSistema, Excepcion, Mensaje)</example>
        /// <param name="Sistema">Int. Identificador del sistema</param>
        /// <param name="Excepcion">Exception. Excepción arrojada por el sistema</param>
        /// <param name="Mensaje">String. Mensaje para humanos</param>
        /// <returns>Retorna Id del registro, si el guardado falla se retorna -1</returns>
        public int WARNING(int Sistema, Exception Excepcion, string Mensaje)
        {
            int Tipo = (int)Tipos.Warning;
            return this.Guardar(Tipo, Sistema, Excepcion, Mensaje);
        }

        /// <summary>
        ///  Guarda registro de LOG de advertencia
        ///  <para>
        ///  <code>Ejemplo. WARNING(1, "Mensaje de error")</code>
        ///  </para>
        /// </summary>
        /// <example>Ejemplo. WARNING(IdentificadorSistema, Mensaje)</example>
        /// <param name="Sistema">Int. Identificador del sistema</param>
        /// <param name="Mensaje">String. Mensaje para humanos</param>
        /// <returns>Retorna Id del registro, si el guardado falla se retorna -1</returns>
        public int WARNING(int Sistema, string Mensaje)
        {
            int Tipo = (int)Tipos.Warning;
            return this.Guardar(Tipo, Sistema, Mensaje);
        }

        /// <summary>
        ///  Guarda registro de LOG de debug
        ///  <para>
        ///  <code>Ejemplo. DEBUG(1, e, "Mensaje de error")</code>
        ///  </para>
        /// </summary>
        /// <example>Ejemplo. DEBUG(IdentificadorSistema, Excepcion, Mensaje)</example>
        /// <param name="Sistema">Int. Identificador del sistema</param>
        /// <param name="Excepcion">Exception. Excepción arrojada por el sistema</param>
        /// <param name="Mensaje">String. Mensaje para humanos</param>
        /// <returns>Retorna Id del registro, si el guardado falla se retorna -1</returns>
        public int DEBUG(int Sistema, Exception Excepcion, string Mensaje)
        {
            int Tipo = (int)Tipos.Debug;
            return this.Guardar(Tipo, Sistema, Excepcion, Mensaje);
        }

        /// <summary>
        ///  Guarda registro de LOG de debug
        ///  <para>
        ///  <code>Ejemplo. DEBUG(1, "Mensaje de error")</code>
        ///  </para>
        /// </summary>
        /// <example>Ejemplo. DEBUG(IdentificadorSistema, Mensaje)</example>
        /// <param name="Sistema">Int. Identificador del sistema</param>
        /// <param name="Mensaje">String. Mensaje para humanos</param>
        /// <returns>Retorna Id del registro, si el guardado falla se retorna -1</returns>
        public int DEBUG(int Sistema, string Mensaje)
        {
            int Tipo = (int)Tipos.Debug;
            return this.Guardar(Tipo, Sistema, Mensaje);
        }

        private int Guardar(int Tipo, int Sistema, string Mensaje)
        {
            int salida = -1;
            try
            {
                DateTime Fecha = DateTime.Now;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["LOGSISTEMAS"].ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand comando = new System.Data.SqlClient.SqlCommand("pa_guardar", conn))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@sistema", Sistema);
                        comando.Parameters.AddWithValue("@tipo", Tipo);
                        comando.Parameters.AddWithValue("@fecha", Fecha);
                        comando.Parameters.AddWithValue("@excepcion", string.Empty);
                        comando.Parameters.AddWithValue("@mensaje", Mensaje);
                        //comando.Parameters.Add("@salida", System.Data.SqlDbType.Bit);
                        var retorno = new System.Data.SqlClient.SqlParameter("@salida", System.Data.SqlDbType.Int);
                        retorno.Direction = System.Data.ParameterDirection.Output;
                        comando.Parameters.Add(retorno);
                        comando.CommandTimeout = 0;

                        conn.Open();
                        comando.ExecuteNonQuery();

                        try
                        {
                            salida = (int)retorno.Value;
                        }
                        catch (Exception)
                        {
                            salida = -1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                salida = -1;
            }

            return salida;
        }

        private int Guardar(int Tipo, int Sistema, Exception Excepcion, string Mensaje)
        {
            int salida = -1;
            try
            {
                DateTime Fecha = DateTime.Now;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["LOGSISTEMAS"].ConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand comando = new System.Data.SqlClient.SqlCommand("pa_guardar", conn))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@sistema", Sistema);
                        comando.Parameters.AddWithValue("@tipo", Tipo);
                        comando.Parameters.AddWithValue("@fecha", Fecha);
                        comando.Parameters.AddWithValue("@excepcion", Excepcion.StackTrace);
                        comando.Parameters.AddWithValue("@mensaje", Mensaje);
                        comando.Parameters.AddWithValue("@mensaje2", Excepcion.GetBaseException().Message);
                        //comando.Parameters.Add("@salida", System.Data.SqlDbType.Bit);
                        var retorno = new System.Data.SqlClient.SqlParameter("@salida", System.Data.SqlDbType.Int);
                        retorno.Direction = System.Data.ParameterDirection.Output;
                        comando.Parameters.Add(retorno);
                        comando.CommandTimeout = 0;

                        conn.Open();
                        comando.ExecuteNonQuery();

                        try
                        {
                            salida = (int)retorno.Value;
                        }
                        catch (Exception)
                        {
                            salida = -1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                salida = -1;
            }

            return salida;
        }
    }
}
