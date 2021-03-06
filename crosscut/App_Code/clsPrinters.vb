Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPrinters
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Name As String = ""
    Dim _HostName As String = ""
    Dim _LocationID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Printers where PrinterID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Printers where PrinterID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Printers")
            If ds.Tables("t_Printers").Rows.Count > 0 Then
                dr = ds.Tables("t_Printers").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List() As DataTable
        Dim ret As New DataTable
        Try
            cm.CommandText = "Select * from t_Printers order by Name"
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
        If Not (dr("PrinterID") Is System.DBNull.Value) Then _ID = dr("PrinterID")
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("HostName") Is System.DBNull.Value) Then _HostName = dr("HostName")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Printers where PrinterID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Printers")
            If ds.Tables("t_Printers").Rows.Count > 0 Then
                dr = ds.Tables("t_Printers").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PrintersInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@HostName", SqlDbType.varchar, 0, "HostName")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PrinterID", SqlDbType.Int, 0, "PrinterID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Printers").NewRow
            End If
            Update_Field("Name", _Name, dr)
            Update_Field("HostName", _HostName, dr)
            Update_Field("LocationID", _LocationID, dr)
            If ds.Tables("t_Printers").Rows.Count < 1 Then ds.Tables("t_Printers").Rows.Add(dr)
            da.Update(ds, "t_Printers")
            _ID = ds.Tables("t_Printers").Rows(0).Item("PrinterID")
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
            oEvents.KeyField = "PrinterID"
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

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property HostName() As String
        Get
            Return _HostName
        End Get
        Set(ByVal value As String)
            _HostName = value
        End Set
    End Property

    Public Property LocationID() As Integer
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Integer)
            _LocationID = value
        End Set
    End Property

    Public Property PrinterID() As Integer
        Get
            Return _ID
        End Get
        Set(value As Integer)
            _id = value
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
