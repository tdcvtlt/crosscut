Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System


Public Class clsEmail
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _Email As String = ""
    Dim _IsPrimary As Boolean = False
    Dim _IsActive As Boolean = False
    Dim _Err As String = ""
    Dim _UserID As Integer = 0

    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dRead As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("", cn)
    End Sub

    Protected Overrides Sub Finalize()
        'If cn.State <> ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        da = Nothing
        ds = Nothing
        dr = Nothing
        dRead = Nothing
        MyBase.Finalize()
    End Sub
    Public Function Get_Table() As DataTable
        Dim dt As New DataTable
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("select EmailID as ID, IsActive as Active, IsPrimary, Email from t_ProspectEmail where prospectid = " & _ProspectID, cn)
        Dim dr As SqlDataReader
        Dim row As DataRow
        Dim i As Integer
        Try
            cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                For i = 0 To dr.VisibleFieldCount - 1
                    dt.Columns.Add(dr.GetName(i))
                Next
                dt.Columns.Add("Dirty")
                row = dt.NewRow
                For i = 0 To dr.VisibleFieldCount - 1
                    row.Item(i) = dr.Item(i)
                Next
                row("Dirty") = False
                dt.Rows.Add(row)
                While dr.Read
                    row = dt.NewRow
                    For i = 0 To dr.VisibleFieldCount - 1
                        row.Item(i) = dr.Item(i)
                    Next
                    row("Dirty") = False
                    dt.Rows.Add(row)
                End While
            End If
            dr.Close()
            cn.Close()

        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try

        Return dt
    End Function
    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.SelectCommand = "Select p.EmailID as ID, p.Email, p.IsPrimary as [Primary], p.IsActive as Active from t_ProspectEmail p where p.prospectid = " & _ProspectID
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Sub Save()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ProspectEmail where EmailID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlcmdbuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "Email")
            If ds.Tables("Email").Rows.Count > 0 Then
                dr = ds.Tables("Email").Rows(0)
            Else
                dr = ds.Tables("Email").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("Email", _Email, dr)
            Update_Field("IsPrimary", _IsPrimary, dr)
            Update_Field("IsActive", _IsActive, dr)

            If ds.Tables("Email").Rows.Count < 1 Then ds.Tables("Email").Rows.Add(dr)

            da.Update(ds, "Email")
            '_ID = ds.Tables("Email").Rows(0).Item("EmailID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Sub Load()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ProspectEmail where EmailID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Email")
            If ds.Tables("Email").Rows.Count > 0 Then
                dr = ds.Tables("Email").Rows(0)
                Set_Values()
            End If

            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        _ID = dr("EmailID")
        _ProspectID = IIf(Not (dr("ProspectID") Is System.DBNull.Value), dr("ProspectID"), 0)
        _Email = IIf(Not (dr("Email") Is System.DBNull.Value), dr("Email"), "")
        _IsPrimary = IIf(Not (dr("IsPrimary") Is System.DBNull.Value), dr("IsPrimary"), False)
        _IsActive = IIf(Not (dr("IsActive") Is System.DBNull.Value), dr("IsActive"), False)
    End Sub

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "EmailID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Public Function Get_Primary_Email_ID(ByVal prosID As Integer) As Integer
        Dim email As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Top 1 emailid from t_ProspectEmail where IsPrimary = 1 and IsActive = 1 and prospectID = " & prosID
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                dRead.Read()
                email = dRead("Emailid")
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return email
    End Function


    Public Function Get_Primary_Email(ByVal prosID As Integer) As String
        Dim email As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Top 1 email from t_ProspectEmail where IsPrimary = 1 and IsActive = 1 and prospectID = " & prosID
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                dRead.Read()
                email = dRead("Email")
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return email
    End Function

    Public Function Verify_Primary_Email(ByVal emailID As Integer, ByVal prosID As Integer) As Boolean
        Dim bValid As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Emails from t_ProspectEmail where isPrimary = 1 and emailID <> " & emailID & " and prospectid = " & prosID
            dRead = cm.ExecuteReader
            dRead.Read()
            If dRead("Emails") = 0 Then
                bValid = True
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property EmailID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = value
        End Set
    End Property

    Public Property IsActive() As Boolean
        Get
            Return _IsActive
        End Get
        Set(ByVal value As Boolean)
            _IsActive = value
        End Set
    End Property

    Public Property IsPrimary() As Boolean
        Get
            Return _IsPrimary
        End Get
        Set(ByVal value As Boolean)
            _IsPrimary = value
        End Set
    End Property

    Public ReadOnly Property Error_Message() As String
        Get
            Return _Err
        End Get
    End Property
End Class
