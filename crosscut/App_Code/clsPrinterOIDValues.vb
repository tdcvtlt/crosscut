Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPrinterOIDValues
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PrinterID As Integer = 0
    Dim _OIDID As Integer = 0
    Dim _DateRead As String = ""
    Dim _Value As String = ""
    Dim _Delta As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PrinterOIDValues where ReadID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PrinterOIDValues where ReadID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PrinterOIDValues")
            If ds.Tables("t_PrinterOIDValues").Rows.Count > 0 Then
                dr = ds.Tables("t_PrinterOIDValues").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List_By_Printer(printerid As Integer) As DataTable
        Dim ret As New DataTable
        Try
            cm.CommandText = "Select v.ReadID, o.OID, v.DateRead,v.Value,v.Delta from t_PrinterOIDValues v left outer join t_PrinterOID o on o.oidid = v.oidid where v.PrinterID =" & printerid
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
        If Not (dr("OIDID") Is System.DBNull.Value) Then _OIDID = dr("OIDID")
        If Not (dr("DateRead") Is System.DBNull.Value) Then _DateRead = dr("DateRead")
        If Not (dr("Value") Is System.DBNull.Value) Then _Value = dr("Value")
        If Not (dr("Delta") Is System.DBNull.Value) Then _Delta = dr("Delta")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PrinterOIDValues where ReadID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PrinterOIDValues")
            If ds.Tables("t_PrinterOIDValues").Rows.Count > 0 Then
                dr = ds.Tables("t_PrinterOIDValues").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PrinterOIDValuesInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PrinterID", SqlDbType.int, 0, "PrinterID")
                da.InsertCommand.Parameters.Add("@OIDID", SqlDbType.int, 0, "OIDID")
                da.InsertCommand.Parameters.Add("@DateRead", SqlDbType.smalldatetime, 0, "DateRead")
                da.InsertCommand.Parameters.Add("@Value", SqlDbType.varchar, 0, "Value")
                da.InsertCommand.Parameters.Add("@Delta", SqlDbType.varchar, 0, "Delta")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ReadID", SqlDbType.Int, 0, "ReadID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PrinterOIDValues").NewRow
            End If
            Update_Field("PrinterID", _PrinterID, dr)
            Update_Field("OIDID", _OIDID, dr)
            Update_Field("DateRead", _DateRead, dr)
            Update_Field("Value", _Value, dr)
            Update_Field("Delta", _Delta, dr)
            If ds.Tables("t_PrinterOIDValues").Rows.Count < 1 Then ds.Tables("t_PrinterOIDValues").Rows.Add(dr)
            da.Update(ds, "t_PrinterOIDValues")
            _ID = ds.Tables("t_PrinterOIDValues").Rows(0).Item("ReadID")
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
            oEvents.KeyField = "ReadID"
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

    Public Property OIDID() As Integer
        Get
            Return _OIDID
        End Get
        Set(ByVal value As Integer)
            _OIDID = value
        End Set
    End Property

    Public Property DateRead() As String
        Get
            Return _DateRead
        End Get
        Set(ByVal value As String)
            _DateRead = value
        End Set
    End Property

    Public Property Value() As String
        Get
            Return _Value
        End Get
        Set(ByVal value As String)
            _Value = value
        End Set
    End Property

    Public Property Delta() As String
        Get
            Return _Delta
        End Get
        Set(ByVal value As String)
            _Delta = value
        End Set
    End Property

    Public Property ReadID() As Integer
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
