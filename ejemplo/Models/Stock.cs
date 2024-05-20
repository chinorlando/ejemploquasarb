using System;
using System.Collections.Generic;

namespace OPERACION_OMM.Models;

public partial class Movimiento
{
    public DateTime Fecha { get; set; }

    public string? Tipo { get; set; }

    public decimal? Importe { get; set; }

    public string? NroCuenta { get; set; }

    public virtual Cuenta? oCuenta { get; set; }
}

public class MovimientoSaldo
{
    public string? NroCuenta { get; set; }
    public decimal? Saldo { get; set; }
}

//-- CREAR BASE DE DATOS
//CREATE DATABASE BD_TRANSACCIONES_OMM;

//-- usar la base de datos
//use BD_TRANSACCIONES_OMM;

//-- crear usuario y contraseña
//CREATE LOGIN USUARIO WITH PASSWORD = 'PASSWORD';
//CREATE USER USUARIO FOR LOGIN USUARIO;
//ALTER ROLE db_owner ADD MEMBER USUARIO;

//--CREACION DE TABLAS
//CREATE TABLE cuenta(
//    nro_cuenta NVARCHAR(14) PRIMARY KEY,
//    tipo CHAR(3) CHECK(TIPO IN ('AHO', 'CTE')),
//    moneda CHAR(3),
//    nombre NVARCHAR(40),
//    saldo DECIMAL(12, 2)
//);

//CREATE TABLE movimiento(
//    fecha DATETIME PRIMARY KEY,
//    tipo char(1) CHECK(TIPO IN ('D', 'A')),
//	importe decimal (12,2),
//	nro_cuenta NVARCHAR(14),
//	CONSTRAINT FK_MOVIMEINTO_CUENTA FOREIGN KEY(nro_cuenta) REFERENCES cuenta(nro_cuenta),
//);

//select* from cuenta;
//select* from movimiento;

//select nro_cuenta, SUM(importe) as saldo from movimiento
//group by nro_cuenta;

//-- procedimiento almacenado
//CREATE PROCEDURE SumarizarMovimientosYActualizarCuentas
//AS
//BEGIN
//    -- Temporalmente deshabilitar la verificación de claves foráneas
//    SET NOCOUNT ON;
//SET XACT_ABORT ON;

//    BEGIN TRY
//        -- Crear una tabla temporal para almacenar las sumas por número de cuenta
//        CREATE TABLE #TempSumaMovimientos (
//            nro_cuenta VARCHAR(50),
//            importe DECIMAL(18, 2)
//        );

//        -- Insertar las sumas de importe por número de cuenta en la tabla temporal
//        INSERT INTO #TempSumaMovimientos (nro_cuenta, importe)
//        SELECT nro_cuenta, SUM(Importe) AS Saldo
//        FROM Movimiento
//        GROUP BY nro_cuenta;

//        -- Actualizar la tabla Cuenta con los saldos calculados
//        UPDATE Cuenta
//        SET Saldo = T.importe
//        FROM Cuenta C
//        JOIN #TempSumaMovimientos T ON C.nro_cuenta = T.nro_cuenta;

//        -- Eliminar la tabla temporal
//        DROP TABLE #TempSumaMovimientos;
        
//        -- Confirmar la transacción
//        COMMIT;
//END TRY
//    BEGIN CATCH
//        -- Si ocurre un error, revertir la transacción
//        ROLLBACK;
//        THROW;
//    END CATCH;
//END;


//CREATE TRIGGER ActualizarSaldoCuenta
//ON movimiento
//AFTER INSERT, UPDATE, DELETE
//AS
//BEGIN
//    EXEC SumarizarMovimientosYActualizarCuentas;
//END;