Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System

Public Class clsNotes
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader
    Dim _NoteID As Integer = 0
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _CreatedBy As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _Note As String = ""
    Dim _DateCreated As String
    Dim _sErr As String = ""
    Dim _UserID As Integer = 0

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Replace", cn)
    End Sub

    Public Sub Load()
        Try
            cn.Open()
            cm.CommandText = "Select n.NoteID, n.KeyField,n.keyvalue, n.Note, n.DateCreated,p.Personnelid, p.Username from t_Note n left outer join t_Personnel p on p.personnelid = n.createdbyid where n.NoteID = " & _NoteID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Notes")
            If ds.Tables("Notes").Rows.Count > 0 Then
                dr = ds.Tables("Notes").Rows(0)
                Fill_Values()
            End If
            cn.Close()
        Catch ex As Exception
            _sErr = ex.ToString
        End Try
    End Sub

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns

        Try
            If _KeyValue > 0 Then
                'ds.SelectCommand = "Select n.NoteID,n.Note, n.DateCreated, p.Username from t_Note n left outer join t_Personnel p on p.personnelid = n.createdbyid where Keyfield='" & _KeyField & "' and Keyvalue = " & _KeyValue & " order by n.noteid desc"
                ds.SelectCommand = "Select n.NoteID,n.Note, n.DateCreated, p.Username from t_Note n left outer join t_Personnel p on p.personnelid = n.createdbyid where Keyfield in (" & String.Join(",", (From e As String In _KeyField.Split(",") Select String.Format("'{0}'", e)).ToArray()) & ") and Keyvalue = " & _KeyValue & " order by n.noteid desc"
            Else
                ds.SelectCommand = "Select n.NoteID,n.Note, n.DateCreated, p.Username from t_Note n left outer join t_Personnel p on p.personnelid = n.createdbyid where 1=2"
            End If
        Catch ex As Exception
            _sErr = ex.ToString
        End Try
        Return ds
    End Function

    Public Function Get_Notes_Table() As DataTable
        Dim dt As New DataTable
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select n.NoteID As ID,n.Note, n.DateCreated, n.CreatedByID, p.Username from t_Note n left outer join t_Personnel p on p.personnelid = n.createdbyid where Keyfield='" & _KeyField & "' and Keyvalue = " & _KeyValue, cn)
        Dim dr As SqlDataReader
        Dim row As DataRow
        Dim i As Integer
        Try
            cn.Open()
            dr = cm.ExecuteReader
            dr.Read()
            For i = 0 To dr.VisibleFieldCount - 1
                dt.Columns.Add(dr.GetName(i))
            Next
            dt.Columns.Add("Dirty")
            If dr.HasRows Then
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
            _sErr = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try

        Return dt
    End Function

    Private Sub Fill_Values()
        _NoteID = dr("NoteID")
        _CreatedBy = dr("UserName") & ""
        _CreatedByID = IIf(Not (dr("PersonnelID") Is System.DBNull.Value), dr("PersonnelID"), 0)
        _Note = dr("Note") & ""
        _DateCreated = IIf(Not (dr("DateCreated") Is System.DBNull.Value), dr("DateCreated"), "")
        _KeyField = dr("KeyField") & ""
        _KeyValue = IIf(Not (dr("KeyValue") Is System.DBNull.Value), dr("KeyValue"), 0)
    End Sub

    Public Sub Save()
        cm.CommandText = "Select * from t_Note where NoteID = " & _NoteID
        da = New SqlDataAdapter(cm)
        ds = New DataSet
        Dim sqlCMD As New SqlCommandBuilder(da)
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            da.Fill(ds, "Note")
            If ds.Tables("Note").Rows.Count > 0 Then
                dr = ds.Tables("Note").Rows(0)
                dr("Note") = _Note
            Else
                dr = ds.Tables("Note").NewRow
                dr("KeyField") = _KeyField
                dr("KeyValue") = _KeyValue
                dr("DateCreated") = Date.Now
                dr("CreatedByID") = _UserID
                dr("Note") = _Note
                ds.Tables("Note").Rows.Add(dr)
            End If

            da.Update(ds, "Note")

        Catch ex As Exception
            _sErr = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            sqlCMD = Nothing

        End Try
    End Sub

    Protected Overrides Sub Finalize()
        'If cn.State <> ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        da = Nothing
        ds = Nothing
        dr = Nothing

        MyBase.Finalize()
    End Sub

    Public Function Get_Body_Notes(ByVal keyField As String, ByVal keyValue As Integer) As String
        Dim body As String
        body = "<table>"
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select n.Note, p.LastName, p.FirstName, n.DateCreated from t_Note n inner join t_Personnel p on n.CreatedByID = p.PersonnelID where n.KeyField = '" & keyField & "' and n.KeyValue = " & keyValue & " order by n.NoteID asc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    body = body & "<tr>"
                    body = body & "<td>" & dread("Note") & "</td>"
                    body = body & "<td>" & dread("LastName") & ", " & dread("FirstName") & "</td>"
                    body = body & "<td>" & CDate(dread("DateCreated")).ToShortDateString & "</td>"
                    body = body & "</tr>"
                Loop
            End If
            dread.Close()
            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _sErr = ex.Message
        End Try
        body = body & "</table>"
        Return body
    End Function

    Public Function Get_Note_Count() As Integer
        Dim notes As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Notes from t_Note where keyfield = '" & _KeyField & "' and keyvalue = '" & _KeyValue & "'"
            dread = cm.ExecuteReader
            dread.Read()
            notes = dread("Notes")
            dread.Close()
        Catch ex As Exception
            _sErr = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return notes
    End Function

    Public ReadOnly Property Error_Message() As String
        Get
            Return _sErr
        End Get
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property Note() As String
        Get
            Return _Note
        End Get
        Set(ByVal value As String)
            _Note = value
        End Set
    End Property

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property CreatedBy() As String
        Get
            Return _CreatedBy
        End Get
        Set(ByVal value As String)
            _CreatedBy = value
        End Set
    End Property

    Public Property NoteID() As Integer
        Get
            Return _NoteID
        End Get
        Set(ByVal value As Integer)
            _NoteID = value
        End Set
    End Property

    Public Property KeyValue() As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
        End Set
    End Property

    Public Property KeyField() As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
        End Set
    End Property
End Class
