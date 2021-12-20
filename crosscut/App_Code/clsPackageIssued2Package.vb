Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackageIssued2Package
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageIssuedID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _DateAdded As String = ""
    Dim _DateRemoved As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackageIssued2Package where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackageIssued2Package where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageIssued2Package")
            If ds.Tables("t_PackageIssued2Package").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageIssued2Package").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PackageIssuedID") Is System.DBNull.Value) Then _PackageIssuedID = dr("PackageIssuedID")
        If Not (dr("PackageID") Is System.DBNull.Value) Then _PackageID = dr("PackageID")
        If Not (dr("DateAdded") Is System.DBNull.Value) Then _DateAdded = dr("DateAdded")
        If Not (dr("DateRemoved") Is System.DBNull.Value) Then _DateRemoved = dr("DateRemoved")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageIssued2Package where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackageIssued2Package")
            If ds.Tables("t_PackageIssued2Package").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageIssued2Package").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageIssued2PackageInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageIssuedID", SqlDbType.int, 0, "PackageIssuedID")
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@DateAdded", SqlDbType.datetime, 0, "DateAdded")
                da.InsertCommand.Parameters.Add("@DateRemoved", SqlDbType.datetime, 0, "DateRemoved")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackageIssued2Package").NewRow
            End If
            Update_Field("PackageIssuedID", _PackageIssuedID, dr)
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("DateAdded", _DateAdded, dr)
            Update_Field("DateRemoved", _DateRemoved, dr)
            If ds.Tables("t_PackageIssued2Package").Rows.Count < 1 Then ds.Tables("t_PackageIssued2Package").Rows.Add(dr)
            da.Update(ds, "t_PackageIssued2Package")
            _ID = ds.Tables("t_PackageIssued2Package").Rows(0).Item("ID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "ID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property PackageIssuedID() As Integer
        Get
            Return _PackageIssuedID
        End Get
        Set(ByVal value As Integer)
            _PackageIssuedID = value
        End Set
    End Property

    Public Property PackageID() As Integer
        Get
            Return _PackageID
        End Get
        Set(ByVal value As Integer)
            _PackageID = value
        End Set
    End Property

    Public Property DateAdded() As String
        Get
            Return _DateAdded
        End Get
        Set(ByVal value As String)
            _DateAdded = value
        End Set
    End Property

    Public Property DateRemoved() As String
        Get
            Return _DateRemoved
        End Get
        Set(ByVal value As String)
            _DateRemoved = value
        End Set
    End Property

    Public Property ID() As Integer
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
        Set(value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property
End Class
