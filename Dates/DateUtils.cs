using System;

namespace Utilities.Dates
{
    public class DateUtils
    {       

        #region Métodos públicos estáticos
        /// <summary>
        /// Obtiene una matriz de Fechas obtenidas de forma pseudo-aleatoria.
        /// </summary>
        /// <param name="numDates">Nº de Fechas deseadas</param>
        /// <returns></returns>
        public static DateTime[] GetRandomDate(int numDates)
         {
               return GetRandomDate(numDates, DateTime.MinValue, DateTime.MaxValue);
           }
        
        
         /// <summary>
         /// Obtiene una matriz de Fechas obtenidas de forma pseudo-aleatoria.
         /// </summary>
         /// <param name="numDates">Nº de Fechas deseadas</param>
         /// <param name="yearInit">Año de inicio del intervalo</param>
         /// <param name="yearEnd">Año de fin del intervalo</param>
         /// <returns></returns>
         public static DateTime[] GetRandomDate(int numDates, int yearInit, int yearEnd)
         {
               return GetRandomDate(numDates, new DateTime(yearInit, 1, 1), new DateTime(yearEnd, 12, 31));
         }

        /// <summary>
        /// genera un numoer de fechas (numDates) asosiadas al rededor de una fecha en especifico(dateAraound) y que varía segun la opcion (option).por defecto la opcion
        /// es uno por lo cual la fecha varía entre 30 días
        /// </summary>
        /// <param name="numDates"></param>
        /// <param name="dateAround"></param>
        /// <param name="option">(default) 1 dias, 2 meses, 3 años</param>
        /// <returns></returns>
        public static DateTime[] GetRandomDate(int numDates, DateTime dateAround, int option = 1)
        {
            switch (option)
            {
                case 2:
                    return GetRandomDate(numDates, new DateTime(dateAround.Year, 1, dateAround.Day), new DateTime(dateAround.Year, 12, dateAround.Day));

                case 3:
                    return GetRandomDate(numDates, new DateTime(dateAround.Year - 5, dateAround.Month, dateAround.Day), new DateTime(dateAround.Year + 5, dateAround.Month, dateAround.Day));

                default:
                    

                    return GetRandomDate(numDates, new DateTime(dateAround.Year, dateAround.Month, 1), new DateTime(dateAround.Year, dateAround.Month, DateTime.DaysInMonth(dateAround.Year,dateAround.Month)));
            }
        }

        /// <summary>
        /// Obtiene una matriz de Fechas obtenidas de forma pseudo-aleatoria.
        /// </summary>
        /// <param name="numDates">Nº de Fechas deseadas</param>
        /// <param name="dateInit">Fecha de inicio del intervalo</param>
        /// <param name="dateEnd">Fecha de fin del intervalo</param>
        /// <returns></returns>
        public static DateTime[] GetRandomDate(int numDates, DateTime dateInit, DateTime dateEnd)
         {
               DateUtils du = new DateUtils();
               return du.getDates(numDates, dateInit, dateEnd);
         }


        #endregion

        #region Private Methods
        private Random seed = new Random(DateTime.Now.Millisecond);
        // Evita la instancia de la clase.
        private DateUtils() { }

        private DateTime[] getDates(int numDates, DateTime dateInit, DateTime dateEnd)
        {
            DateTime[] lst = new DateTime[numDates];
            for (int i = 0; i < numDates; i++)
            {
                // Obtenemos el intervalo de tiempo
                TimeSpan interval = dateEnd.Subtract(dateInit);
                // Se calcula el número de días
                int randomMax = (int)interval.TotalDays;
                // Se obtiene un número aleatorio
                long randomValue = seed.Next(0, randomMax);
                // Se le añade a la fecha inicial
                lst[i] = dateInit.AddDays(randomValue);
            }
            return lst;
        }
        #endregion

    }
}
