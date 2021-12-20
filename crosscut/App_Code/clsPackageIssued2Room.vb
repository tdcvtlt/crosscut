Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackageIssued2Room
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PkgIss2PkgID As Integer = 0
    Dim _RoomID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PackageIssued2Room where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PackageIssued2Room where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PackageIssued2Room")
            If ds.Tables("t_PackageIssued2Room").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageIssued2Room").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PkgIss2PkgID") Is System.DBNull.Value) Then _PkgIss2PkgID = dr("PkgIss2PkgID")
        If Not (dr("RoomID") Is System.DBNull.Value) Then _RoomID = dr("RoomID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PackageIssued2Room where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PackageIssued2Room")
            If ds.Tables("t_PackageIssued2Room").Rows.Count > 0 Then
                dr = ds.Tables("t_PackageIssued2Room").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PackageIssued2RoomInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PkgIss2PkgID", SqlDbType.int, 0, "PkgIss2PkgID")
                da.InsertCommand.Parameters.Add("@RoomID", SqlDbType.int, 0, "RoomID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PackageIssued2Room").NewRow
            End If
            Update_Field("PkgIss2PkgID", _PkgIss2PkgID, dr)
            Update_Field("RoomID", _RoomID, dr)
            If ds.Tables("t_PackageIssued2Room").Rows.Count < 1 Then ds.Tables("t_PackageIssued2Room").Rows.Add(dr)
            da.Update(ds, "t_PackageIssued2Room")
            _ID = ds.Tables("t_PackageIssued2Room").Rows(0).Item("ID")
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

    Public Property PkgIss2PkgID() As Integer
        Get
            Return _PkgIss2PkgID
        End Get
        Set(ByVal value As Integer)
            _PkgIss2PkgID = value
        End Set
    End Property

    Public Property RoomID() As Integer
        Get
            Return _RoomID
        End Get
        Set(ByVal value As Integer)
            _RoomID = value
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
