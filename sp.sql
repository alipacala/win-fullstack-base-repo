CREATE OR ALTER PROCEDURE dbo.usp_Cliente_Buscar
    @Termino VARCHAR(100),
    @Limite INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Limite)
        Id,
        TipoDocumento,
        NumeroDocumento,
        RazonSocial,
        Segmento,
        Correo
    FROM dbo.Cliente
    WHERE Activo = 1
      AND (NumeroDocumento LIKE @Termino + '%' OR RazonSocial LIKE '%' + @Termino + '%')      

    ORDER BY RazonSocial ASC;
END;