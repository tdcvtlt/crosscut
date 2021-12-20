Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPersonnelTimeCards
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _Swipe As String = ""
    Dim _Active As Boolean = False
    Dim _Description As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PersonnelTimeCards where CardID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PersonnelTimeCards where CardID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelTimeCards")
            If ds.Tables("t_PersonnelTimeCards").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelTimeCards").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("Swipe") Is System.DBNull.Value) Then _Swipe = dr("Swipe")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PersonnelTimeCards where CardID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelTimeCards")
            If ds.Tables("t_PersonnelTimeCards").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelTimeCards").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PersonnelTimeCardsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@Swipe", SqlDbType.varchar, 0, "Swipe")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.varchar, 0, "Description")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@CardID", SqlDbType.Int, 0, "CardID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PersonnelTimeCards").NewRow
            End If
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("Swipe", _Swipe, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("Description", _Description, dr)
            If ds.Tables("t_PersonnelTimeCards").Rows.Count < 1 Then ds.Tables("t_PersonnelTimeCards").Rows.Add(dr)
            da.Update(ds, "t_PersonnelTimeCards")
            _ID = ds.Tables("t_PersonnelTimeCards").Rows(0).Item("CardID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
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
            oEvents.KeyField = "CardID"
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

    Public Function List_Cards(ByVal persID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select CardID, Description, Active from t_PersonnelTimeCards where personnelid = '" & persID & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Validate_Code(ByVal swipe As String, ByVal persID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case When Count(*) is null then 0 else count(*) end as swipes from t_PersonnelTimeCards where swipe = '" & swipe & "' and personnelid <> " & persID & " and active = '1'"
            dread = cm.ExecuteReader
            dread.Read()
            If dread("swipes") > 0 Then
                bValid = False
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
            bValid = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function get_Personnel_ID(ByVal swipe As String) As Integer
        Dim persID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select p.PersonnelID from t_PersonnelTimeCards pc inner join t_Personnel p on pc.PersonnelID = p.PersonnelID inner join t_ComboItems ps on p.StatusID = ps.CombOitemID where pc.swipe = '" & swipe & "' and pc.active = '1' and ps.CombOitem like 'Active%'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                persID = dread("PersonnelID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return persID
    End Function
    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property Swipe() As String
        Get
            Return _Swipe
        End Get
        Set(ByVal value As String)
            _Swipe = value
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

    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
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

    Public Property CardID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
