Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsDocumentGroups
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _GroupName As String = ""
    Dim _Description As String = ""
    Dim _TypeID As Integer = 0
    Dim _SubTypeID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _Active As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_DocumentGroups where DocumentGroupID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_DocumentGroups where DocumentGroupID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_DocumentGroups")
            If ds.Tables("t_DocumentGroups").Rows.Count > 0 Then
                dr = ds.Tables("t_DocumentGroups").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List(ByVal dgtype As String) As SqlDataSource
        Dim ds As New SqlDataSource(Resources.Resource.cns, "Select dg.DocumentGroupID as ID, dg.GroupName as Name, dg.Description from t_DocumentGroups dg inner join t_ComboItems dgt on dg.TypeID = dgt.ComboItemID where dg.Active = 1 and dgt.ComboItem = '" & dgtype & "' order by GroupName")
        Return ds
        ds = Nothing
    End Function

    Private Sub Set_Values()
        If Not (dr("GroupName") Is System.DBNull.Value) Then _GroupName = dr("GroupName")
        If Not (dr("Description") Is System.DBNull.Value) Then _Description = dr("Description")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("SubTypeID") Is System.DBNull.Value) Then _SubTypeID = dr("SubTypeID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_DocumentGroups where DocumentGroupID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_DocumentGroups")
            If ds.Tables("t_DocumentGroups").Rows.Count > 0 Then
                dr = ds.Tables("t_DocumentGroups").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_DocumentGroupsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@GroupName", SqlDbType.varchar, 0, "GroupName")
                da.InsertCommand.Parameters.Add("@Description", SqlDbType.varchar, 0, "Description")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@SubTypeID", SqlDbType.Int, 0, "SubTypeID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@DocumentGroupID", SqlDbType.Int, 0, "DocumentGroupID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_DocumentGroups").NewRow
            End If
            Update_Field("GroupName", _GroupName, dr)
            Update_Field("Description", _Description, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("SubTypeID", _SubTypeID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_DocumentGroups").Rows.Count < 1 Then ds.Tables("t_DocumentGroups").Rows.Add(dr)
            da.Update(ds, "t_DocumentGroups")
            _ID = ds.Tables("t_DocumentGroups").Rows(0).Item("DocumentGroupID")
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
            oEvents.KeyField = "DocumentGroupID"
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

    Public Property GroupName() As String
        Get
            Return _GroupName
        End Get
        Set(ByVal value As String)
            _GroupName = value
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

    Public Property TypeID() As Integer
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Integer)
            _TypeID = value
        End Set
    End Property

    Public Property SubTypeID() As Integer
        Get
            Return _SubTypeID
        End Get
        Set(ByVal value As Integer)
            _SubTypeID = value
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

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property


    Public Property DocumentGroupID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class

