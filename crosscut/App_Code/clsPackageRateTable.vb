Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackageRateTable
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageRateID As Integer = 0
    Dim _DateAllocated As String = ""
    Dim _Rate As Decimal = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackageRateTable where PackageRateTableID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackageRateTable where PackageRateTableID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageRateTable")
            If ds.Tables("t_PackageRateTable").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageRateTable").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PackageRateID") Is System.DBNull.Value) Then _PackageRateID = dr("PackageRateID")
        If Not (dr("DateAllocated") Is System.DBNull.Value) Then _DateAllocated = dr("DateAllocated")
        If Not (dr("Rate") Is System.DBNull.Value) Then _Rate = dr("Rate")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageRateTable where PackageRateTableID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackageRateTable")
            If ds.Tables("t_PackageRateTable").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageRateTable").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageRateTableInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageRateID", SqlDbType.int, 0, "PackageRateID")
                da.InsertCommand.Parameters.Add("@DateAllocated", SqlDbType.datetime, 0, "DateAllocated")
                da.InsertCommand.Parameters.Add("@Rate", SqlDbType.money, 0, "Rate")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PackageRateTableID", SqlDbType.Int, 0, "PackageRateTableID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackageRateTable").NewRow
            End If
            Update_Field("PackageRateID", _PackageRateID, dr)
            Update_Field("DateAllocated", _DateAllocated, dr)
            Update_Field("Rate", _Rate, dr)
            If ds.Tables("t_PackageRateTable").Rows.Count < 1 Then ds.Tables("t_PackageRateTable").Rows.Add(dr)
            da.Update(ds, "t_PackageRateTable")
            _ID = ds.Tables("t_PackageRateTable").Rows(0).Item("PackageRateTableID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "PackageRateTableID"
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

    Public Property PackageRateID() As Integer
        Get
            Return _PackageRateID
        End Get
        Set(ByVal value As Integer)
            _PackageRateID = value
        End Set
    End Property

    Public Property DateAllocated() As String
        Get
            Return _DateAllocated
        End Get
        Set(ByVal value As String)
            _DateAllocated = value
        End Set
    End Property

    Public Property Rate() As Decimal
        Get
            Return _Rate
        End Get
        Set(ByVal value As Decimal)
            _Rate = value
        End Set
    End Property

    Public Property PackageRateTableID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property

End Class
