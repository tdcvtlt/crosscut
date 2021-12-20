Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPrinterOID
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PrinterID As Integer = 0
    Dim _OID As String = ""
    Dim _Description As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PrinterOID where OIDID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PrinterOID where OIDID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PrinterOID")
            If ds.Tables("t_PrinterOID").Rows.Count > 0 Then
                dr = ds.Tables("t_PrinterOID").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List_By_Printer(printerid As Integer) As DataTable
        Dim ret As New DataTable
        Try
            cm.CommandText = "Select * from t_PrinterOID where PrinterID =" & printerid
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "List")
            ret = ds.Tables("List")
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ret
    End Function

    Private Sub Set_Values()
        If Not (dr("PrinterID") Is System.DBNull.Value) Then _PrinterID = dr("PrinterID")
        If Not (dr("OID") Is System.DBNull.Value) Then _OID = dr("OID")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PrinterOID where OIDID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PrinterOID")
            If ds.Tables("t_PrinterOID").Rows.Count > 0 Then
                dr = ds.Tables("t_PrinterOID").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PrinterOIDInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PrinterID", SqlDbType.int, 0, "PrinterID")
                da.InsertCommand.Parameters.Add("@OID", SqlDbType.varchar, 0, "OID")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.varchar, 0, "Description")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@OIDID", SqlDbType.Int, 0, "OIDID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PrinterOID").NewRow
            End If
            Update_Field("PrinterID", _PrinterID, dr)
            Update_Field("OID", _OID, dr)
            Update_Field("Description", _Description, dr)
            If ds.Tables("t_PrinterOID").Rows.Count < 1 Then ds.Tables("t_PrinterOID").Rows.Add(dr)
            da.Update(ds, "t_PrinterOID")
            _ID = ds.Tables("t_PrinterOID").Rows(0).Item("OIDID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "OIDID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property PrinterID() As Integer
        Get
            Return _PrinterID
        End Get
        Set(ByVal value As Integer)
            _PrinterID = value
        End Set
    End Property

    Public Property OID() As String
        Get
            Return _OID
        End Get
        Set(ByVal value As String)
            _OID = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Public Property OIDID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _ID = value
        End Set
    End Property
    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property
End Class
