using System;

public class HabitacionHotel
{
    private int numero;
    private string tipo;
    private double precioPorNoche;
    private bool disponible;
    private string clienteActual;

    public int Numero => numero; 

    public string Tipo
    {
        get => tipo;
        set
        {
            if (value == "Individual" || value == "Doble" || value == "Suite")
            {
                tipo = value;
            }
            else
            {
                throw new ArgumentException("El tipo de habitación debe ser 'Individual', 'Doble' o 'Suite'.");
            }
        }
    }

    public double PrecioPorNoche
    {
        get => precioPorNoche;
        set
        {
            if (value > 0)
            {
                precioPorNoche = value;
            }
            else
            {
                throw new ArgumentException("El precio por noche debe ser mayor a 0.");
            }
        }
    }

    public bool Disponible => disponible; 
    public string ClienteActual => clienteActual; 

    public HabitacionHotel(int numero, string tipo, double precioPorNoche)
    {
        this.numero = numero;
        Tipo = tipo; 
        PrecioPorNoche = precioPorNoche; 
        this.disponible = true;
        this.clienteActual = "";
    }

    public double Reservar(string nombreCliente, int noches)
    {
        if (disponible)
        {
            disponible = false;
            clienteActual = nombreCliente;
            return precioPorNoche * noches; 
        }
        return -1; 
    }

    public bool Liberar()
    {
        if (!disponible)
        {
            disponible = true;
            clienteActual = "";
            return true; 
        }
        return false; 
    }

    public string MostrarInfo()
    {
        string estado = disponible ? "Disponible" : $"Ocupada por {clienteActual}";
        return $"Habitación {numero} - Tipo: {tipo} - Precio por noche: ${precioPorNoche:N0} - Estado: {estado}";
    }

    // 1.A. Aplicar Descuento con validación para que no quede en negativo o cero
    public void AplicarDescuento(int porcentaje)
    {
        double descuento = precioPorNoche * (porcentaje / 100.0);
        double nuevoPrecio = precioPorNoche - descuento;
        
        if (nuevoPrecio > 0)
        {
            precioPorNoche = nuevoPrecio;
        }
        else
        {
            Console.WriteLine($"[AVISO] No se pudo aplicar el descuento del {porcentaje}% a la habitación {numero}: el precio resultante sería menor o igual a cero.");
        }
    }

    // 1.B. Obtener Datos Resumen
    public string ObtenerDatosResumen()
    {
        string estado = disponible ? "Disponible" : $"Ocupada por: {clienteActual}";
        return $"[{numero}] {tipo} - ${precioPorNoche:N0} - {estado}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        HabitacionHotel hab101 = new HabitacionHotel(101, "Individual", 70000);
        HabitacionHotel hab202 = new HabitacionHotel(202, "Doble", 100000);
        HabitacionHotel hab303 = new HabitacionHotel(303, "Suite", 180000);
        HabitacionHotel hab304 = new HabitacionHotel(304, "Individual", 80000);
        
        Console.WriteLine("=== ESTADO INICIAL DE LAS HABITACIONES ===");
        Console.WriteLine(hab101.MostrarInfo());
        Console.WriteLine(hab202.MostrarInfo());
        Console.WriteLine(hab303.MostrarInfo());
        Console.WriteLine(hab304.MostrarInfo());
        Console.WriteLine();

        Console.WriteLine("=== SIMULACIÓN DE OPERACIONES ===");

        double costo101 = hab101.Reservar("Ivan Camargo", 3);
        Console.WriteLine($"* Se reservó la 101 para Ivan Camargo (3 noches). Costo total: ${costo101:N0}");

        double costo202 = hab202.Reservar("Dayanna peña", 2);
        Console.WriteLine($"* Se reservó la 202 para Dayanna peña (2 noches). Costo total: ${costo202:N0}");

        double intentoFallido = hab101.Reservar("Otro Cliente", 1);
        Console.WriteLine($"* Intento de reservar nuevamente la 101: Resultado {intentoFallido} (Fallo esperado)");

        bool liberada = hab101.Liberar();
        Console.WriteLine($"* Liberando la habitación 101... ¿Éxito? {liberada}");

        hab303.AplicarDescuento(10);
        Console.WriteLine("* Se aplicó un 10% de descuento a la habitación 303 (Suite).");

        double costoCarlos = hab101.Reservar("Enrique Gomez", 1);
        Console.WriteLine($"* Se reservó la 101 para Enrique Gomez (1 noche). Costo total: ${costoCarlos:N0}");
        Console.WriteLine();

        double costo304 = hab304.Reservar("Martin Lopez", 5);
        Console.WriteLine($"* Se reservó la habitación 304 para Martin Lopez (5 noches) Costo total: ${costo304:N0}");
        Console.WriteLine();

        Console.WriteLine("=== ESTADO FINAL DE LAS HABITACIONES ===");
        HabitacionHotel[] listaHabitaciones = { hab101, hab202, hab303, hab304 };
        foreach (var hab in listaHabitaciones)
        {
            Console.WriteLine(hab.ObtenerDatosResumen());
        }
        Console.WriteLine();

        // 2.A y 2.B: Métricas finales con ciclo foreach
        int contadorOcupadas = 0;
        int contadorLibres = 0;
        double sumaPrecios = 0;

        foreach (var hab in listaHabitaciones)
        {
            if (!hab.Disponible)
            {
                contadorOcupadas++;
            }
            else
            {
                contadorLibres++;
            }

            sumaPrecios += hab.PrecioPorNoche;
        }

        double precioPromedio = sumaPrecios / listaHabitaciones.Length;

        Console.WriteLine("=== RESUMEN Y MÉTRICAS DEL HOTEL ===");
        Console.WriteLine($"* Cantidad de habitaciones ocupadas: {contadorOcupadas}");
        Console.WriteLine($"* Cantidad de habitaciones libres: {contadorLibres}");
        Console.WriteLine($"* Precio promedio por noche en el hotel: ${precioPromedio:N2}");
    }
}
