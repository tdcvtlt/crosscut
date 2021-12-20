Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPersonnelSecurityRequest
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _StatusID As Integer = 0
    Dim _TypeID As Integer = 0
    Dim _EmailOption As Integer = 0
    Dim _EmailDomainID As Integer = 0
    Dim _RequestedByID As Integer = 0
    Dim _RequestedDueDate As String = ""
    Dim _DateCreated As String = ""
    Dim _Err As String = ""
    Dim _PhoneTypeID As Integer = 0
    Dim _DID As Boolean = False
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PersonnelSecurityRequest where RequestID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PersonnelSecurityRequest where RequestID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelSecurityRequest")
            If ds.Tables("t_PersonnelSecurityRequest").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelSecurityRequest").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("EmailOption") Is System.DBNull.Value) Then _EmailOption = dr("EmailOption")
        If Not (dr("EmailDomainID") Is System.DBNull.Value) Then _EmailDomainID = dr("EmailDomainID")
        If Not (dr("RequestedByID") Is System.DBNull.Value) Then _RequestedByID = dr("RequestedByID")
        If Not (dr("RequestedDueDate") Is System.DBNull.Value) Then _RequestedDueDate = dr("RequestedDueDate")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("PhoneTypeID") Is System.DBNull.Value) Then _PhoneTypeID = dr("PhoneTypeID")
        If Not (dr("DID") Is System.DBNull.Value) Then _DID = dr("DID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PersonnelSecurityRequest where RequestID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelSecurityRequest")
            If ds.Tables("t_PersonnelSecurityRequest").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelSecurityRequest").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PersonnelSecurityRequestInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@EmailOption", SqlDbType.int, 0, "EmailOption")
                da.InsertCommand.Parameters.Add("@EmailDomainID", SqlDbType.int, 0, "EmailDomainID")
                da.InsertCommand.Parameters.Add("@RequestedByID", SqlDbType.int, 0, "RequestedByID")
                da.InsertCommand.Parameters.Add("@RequestedDueDate", SqlDbType.datetime, 0, "RequestedDueDate")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@PhoneTypeID", SqlDbType.Int, 0, "PhoneTypeID")
                da.InsertCommand.Parameters.Add("@DID", SqlDbType.Bit, 0, "DID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@RequestID", SqlDbType.Int, 0, "RequestID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PersonnelSecurityRequest").NewRow
            End If
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("EmailOption", _EmailOption, dr)
            Update_Field("EmailDomainID", _EmailDomainID, dr)
            Update_Field("RequestedByID", _RequestedByID, dr)
            Update_Field("RequestedDueDate", _RequestedDueDate, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("PhoneTypeID", _PhoneTypeID, dr)
            Update_Field("DID", _DID, dr)
            If ds.Tables("t_PersonnelSecurityRequest").Rows.Count < 1 Then ds.Tables("t_PersonnelSecurityRequest").Rows.Add(dr)
            da.Update(ds, "t_PersonnelSecurityRequest")
            _ID = ds.Tables("t_PersonnelSecurityRequest").Rows(0).Item("RequestID")
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
            oEvents.KeyField = "RequestID"
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

    Public Function List(ByVal filter As String) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        If filter = "" Then
            ds.SelectCommand = "Select RequestID as ID, r.DateCreated, rt.ComboItem as Type, p.FirstName + ' ' + p.LastName as Personnel, rs.ComboItem as Status, rb.FirstName + ' ' + rb.LastName as RequestedBy, r.RequestedDueDate from t_PersonnelSecurityRequest r inner join t_ComboItems rt on r.TypeID = rt.ComboitemID inner join t_ComboItems rs on r.StatusID = rs.ComboItemID inner join t_personnel p on r.PersonnelID = p.Personnelid inner join t_Personnel rb on r.RequestedByID = rb.PersonnelID where rs.Comboitem <> 'Complete' order by RequestID asc"
        Else
            ds.SelectCommand = "Select RequestID as ID, r.DateCreated, rt.ComboItem as Type, p.FirstName + ' ' + p.LastName as Personnel, rs.ComboItem as Status, rb.FirstName + ' ' + rb.LastName as RequestedBy, r.RequestedDueDate from t_PersonnelSecurityRequest r inner join t_ComboItems rt on r.TypeID = rt.ComboitemID inner join t_ComboItems rs on r.StatusID = rs.ComboItemID inner join t_personnel p on r.PersonnelID = p.Personnelid inner join t_Personnel rb on r.RequestedByID = rb.PersonnelID where r.RequestID = " & filter & " order by RequestID desc"
        End If
        Return ds
    End Function

    Public Function List_Requests(ByVal persID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select r.RequestID as ID, rt.ComboItem as Type, rs.ComboItem as Status from t_PersonnelSecurityRequest r inner join t_Comboitems rt on r.TypeID = rt.ComboItemID inner join t_ComboItems rs on r.StatusID = rs.ComboitemID where r.PersonnelID = " & persID
        Return ds
    End Function

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            _StatusID = value
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

    Public Property EmailOption() As Integer
        Get
            Return _EmailOption
        End Get
        Set(ByVal value As Integer)
            _EmailOption = value
        End Set
    End Property

    Public Property EmailDomainID() As Integer
        Get
            Return _EmailDomainID
        End Get
        Set(ByVal value As Integer)
            _EmailDomainID = value
        End Set
    End Property

    Public Property RequestedByID() As Integer
        Get
            Return _RequestedByID
        End Get
        Set(ByVal value As Integer)
            _RequestedByID = value
        End Set
    End Property

    Public Property RequestedDueDate() As String
        Get
            Return _RequestedDueDate
        End Get
        Set(ByVal value As String)
            _RequestedDueDate = value
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

    Public Property PhoneTypeID() As Integer
        Get
            Return _PhoneTypeID
        End Get
        Set(ByVal value As Integer)
            _PhoneTypeID = value
        End Set
    End Property

    Public Property DID() As Boolean
        Get
            Return _DID
        End Get
        Set(ByVal value As Boolean)
            _DID = value
        End Set
    End Property

    Public Property RequestID() As Integer
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
