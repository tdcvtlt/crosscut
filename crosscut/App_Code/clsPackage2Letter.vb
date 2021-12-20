Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPackage2Letter
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PackageID As Integer = 0
    Dim _PackageLetterID As Integer = 0
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim _Subject As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Package2Letter where Package2LetterID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Package2Letter where Package2LetterID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Package2Letter")
            If ds.Tables("t_Package2Letter").Rows.Count > 0 Then
                dr = ds.Tables("t_Package2Letter").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PackageID") Is System.DBNull.Value) Then _PackageID = dr("PackageID")
        If Not (dr("PackageLetterID") Is System.DBNull.Value) Then _PackageLetterID = dr("PackageLetterID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("Subject") Is System.DBNull.Value) Then _Subject = dr("Subject")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Package2Letter where Package2LetterID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Package2Letter")
            If ds.Tables("t_Package2Letter").Rows.Count > 0 Then
                dr = ds.Tables("t_Package2Letter").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Package2LetterInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PackageID", SqlDbType.int, 0, "PackageID")
                da.InsertCommand.Parameters.Add("@PackageLetterID", SqlDbType.int, 0, "PackageLetterID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@Subject", SqlDbType.VarChar, 0, "Subject")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Package2LetterID", SqlDbType.Int, 0, "Package2LetterID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Package2Letter").NewRow
            End If
            Update_Field("PackageID", _PackageID, dr)
            Update_Field("PackageLetterID", _PackageLetterID, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("Subject", _Subject, dr)
            If ds.Tables("t_Package2Letter").Rows.Count < 1 Then ds.Tables("t_Package2Letter").Rows.Add(dr)
            da.Update(ds, "t_Package2Letter")
            _ID = ds.Tables("t_Package2Letter").Rows(0).Item("Package2LetterID")
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
            oEvents.KeyField = "Package2LetterID"
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

    Public Function Get_Package_Letters(ByVal pkgID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select pl.Package2LetterID as ID, l.Name, pl.Active from t_Package2Letter pl inner join t_packageletters l on pl.PackageLetterID = l.PackageLetterID where pl.PackageID = " & pkgID
        Return ds
    End Function


    Public Function Get_Letter_Content(ByVal packageID As Integer) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("Subject")
        dt.Columns.Add("Content")
        If cn.State <> ConnectionState.Open Then cn.Open()
        cm.CommandText = "Select pl.LetterContent, pl2.Subject from t_Package2Letter pl2 inner join t_packageLetters pl on pl2.PackageLetterID = pl.PackageLetterID where pl2.PackageID = " & packageID & " and Active = 1"
        dread = cm.ExecuteReader
        If dread.HasRows Then
            Do While dread.Read
                dr = dt.NewRow
                dr("Subject") = dread("Subject")
                dr("Content") = dread("LetterContent")
                dt.Rows.Add(dr)
            Loop
        End If
        dread.Close()
        If cn.State <> ConnectionState.Closed Then cn.Close()
        Return dt
    End Function
    Public Property PackageID() As Integer
        Get
            Return _PackageID
        End Get
        Set(ByVal value As Integer)
            _PackageID = value
        End Set
    End Property

    Public Property PackageLetterID() As Integer
        Get
            Return _PackageLetterID
        End Get
        Set(ByVal value As Integer)
            _PackageLetterID = value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

    Public Property Package2LetterID() As Integer
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

    Public Property Subject() As String
        Get
            Return _Subject
        End Get
        Set(ByVal value As String)
            _Subject = value
        End Set
    End Property
End Class
