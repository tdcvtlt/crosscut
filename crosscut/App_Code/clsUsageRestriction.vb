Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsUsageRestriction
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _Name As String = ""
    Dim _MinStayFlag As Boolean = False
    Dim _MinStay As Integer = 0
    Dim _AllowDateRangeFlag As Boolean = False
    Dim _DenyDateRangeFlag As Boolean = False
    Dim _StartDate As String = ""
    Dim _EndDate As String = ""
    Dim _DaysOutFlag As Boolean = False
    Dim _MinDaysOut As Integer = 0
    Dim _MaxDaysOut As Integer = 0
    Dim _SeasonUseFlag As Boolean = False
    Dim _SeasonID As Integer = 0
    Dim _SeasonDenyFlag As Boolean = False
    Dim _DenyAll As Boolean = False
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_UsageRestriction where UsageRestrictionID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_UsageRestriction where UsageRestrictionID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_UsageRestriction")
            If ds.Tables("t_UsageRestriction").Rows.Count > 0 Then
                dr = ds.Tables("t_UsageRestriction").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("Name") Is System.DBNull.Value) Then _Name = dr("Name")
        If Not (dr("MinStayFlag") Is System.DBNull.Value) Then _MinStayFlag = dr("MinStayFlag")
        If Not (dr("MinStay") Is System.DBNull.Value) Then _MinStay = dr("MinStay")
        If Not (dr("AllowDateRangeFlag") Is System.DBNull.Value) Then _AllowDateRangeFlag = dr("AllowDateRangeFlag")
        If Not (dr("DenyDateRangeFlag") Is System.DBNull.Value) Then _DenyDateRangeFlag = dr("DenyDateRangeFlag")
        If Not (dr("StartDate") Is System.DBNull.Value) Then _StartDate = dr("StartDate")
        If Not (dr("EndDate") Is System.DBNull.Value) Then _EndDate = dr("EndDate")
        If Not (dr("DaysOutFlag") Is System.DBNull.Value) Then _DaysOutFlag = dr("DaysOutFlag")
        If Not (dr("MinDaysOut") Is System.DBNull.Value) Then _MinDaysOut = dr("MinDaysOut")
        If Not (dr("MaxDaysOut") Is System.DBNull.Value) Then _MaxDaysOut = dr("MaxDaysOut")
        If Not (dr("SeasonUseFlag") Is System.DBNull.Value) Then _SeasonUseFlag = dr("SeasonUseFlag")
        If Not (dr("SeasonID") Is System.DBNull.Value) Then _SeasonID = dr("SeasonID")
        If Not (dr("SeasonDenyFlag") Is System.DBNull.Value) Then _SeasonDenyFlag = dr("SeasonDenyFlag")
        If Not (dr("DenyAll") Is System.DBNull.Value) Then _DenyAll = dr("DenyAll")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_UsageRestriction where UsageRestrictionID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_UsageRestriction")
            If ds.Tables("t_UsageRestriction").Rows.Count > 0 Then
                dr = ds.Tables("t_UsageRestriction").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_UsageRestrictionInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@Name", SqlDbType.varchar, 0, "Name")
                da.InsertCommand.Parameters.Add("@MinStayFlag", SqlDbType.bit, 0, "MinStayFlag")
                da.InsertCommand.Parameters.Add("@MinStay", SqlDbType.int, 0, "MinStay")
                da.InsertCommand.Parameters.Add("@AllowDateRangeFlag", SqlDbType.bit, 0, "AllowDateRangeFlag")
                da.InsertCommand.Parameters.Add("@DenyDateRangeFlag", SqlDbType.bit, 0, "DenyDateRangeFlag")
                da.InsertCommand.Parameters.Add("@StartDate", SqlDbType.datetime, 0, "StartDate")
                da.InsertCommand.Parameters.Add("@EndDate", SqlDbType.datetime, 0, "EndDate")
                da.InsertCommand.Parameters.Add("@DaysOutFlag", SqlDbType.bit, 0, "DaysOutFlag")
                da.InsertCommand.Parameters.Add("@MinDaysOut", SqlDbType.int, 0, "MinDaysOut")
                da.InsertCommand.Parameters.Add("@MaxDaysOut", SqlDbType.int, 0, "MaxDaysOut")
                da.InsertCommand.Parameters.Add("@SeasonUseFlag", SqlDbType.bit, 0, "SeasonUseFlag")
                da.InsertCommand.Parameters.Add("@SeasonID", SqlDbType.int, 0, "SeasonID")
                da.InsertCommand.Parameters.Add("@SeasonDenyFlag", SqlDbType.bit, 0, "SeasonDenyFlag")
                da.InsertCommand.Parameters.Add("@DenyAll", SqlDbType.bit, 0, "DenyAll")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@UsageRestrictionID", SqlDbType.Int, 0, "UsageRestrictionID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_UsageRestriction").NewRow
            End If
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("Name", _Name, dr)
            Update_Field("MinStayFlag", _MinStayFlag, dr)
            Update_Field("MinStay", _MinStay, dr)
            Update_Field("AllowDateRangeFlag", _AllowDateRangeFlag, dr)
            Update_Field("DenyDateRangeFlag", _DenyDateRangeFlag, dr)
            Update_Field("StartDate", _StartDate, dr)
            Update_Field("EndDate", _EndDate, dr)
            Update_Field("DaysOutFlag", _DaysOutFlag, dr)
            Update_Field("MinDaysOut", _MinDaysOut, dr)
            Update_Field("MaxDaysOut", _MaxDaysOut, dr)
            Update_Field("SeasonUseFlag", _SeasonUseFlag, dr)
            Update_Field("SeasonID", _SeasonID, dr)
            Update_Field("SeasonDenyFlag", _SeasonDenyFlag, dr)
            Update_Field("DenyAll", _DenyAll, dr)
            If ds.Tables("t_UsageRestriction").Rows.Count < 1 Then ds.Tables("t_UsageRestriction").Rows.Add(dr)
            da.Update(ds, "t_UsageRestriction")
            _ID = ds.Tables("t_UsageRestriction").Rows(0).Item("UsageRestrictionID")
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
            oEvents.KeyField = "UsageRestrictionID"
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

    Public Function Get_Restrictions() As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select UsageRestrictionID, Name from t_UsageRestriction order by Name asc"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Check_Restrictor(ByVal name As String, ByVal ID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            If ID = 0 Then
                cm.CommandText = "Select Case when Count(*) is null then 0 else count(*) end as Restrictors from t_UsageRestriction where Name = '" & name & "'"
            Else
                cm.CommandText = "Select Case when Count(*) is null then 0 else count(*) end as Restrictors from t_UsageRestriction where Name = '" & name & "' and UsageRestrictionID <> '" & ID & "'"
            End If
            dread = cm.ExecuteReader
            dread.Read()
            If dread("Restrictors") > 0 Then
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
    Public Property LocationID() As Integer
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Integer)
            _LocationID = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Public Property MinStayFlag() As Boolean
        Get
            Return _MinStayFlag
        End Get
        Set(ByVal value As Boolean)
            _MinStayFlag = value
        End Set
    End Property

    Public Property MinStay() As Integer
        Get
            Return _MinStay
        End Get
        Set(ByVal value As Integer)
            _MinStay = value
        End Set
    End Property

    Public Property AllowDateRangeFlag() As Boolean
        Get
            Return _AllowDateRangeFlag
        End Get
        Set(ByVal value As Boolean)
            _AllowDateRangeFlag = value
        End Set
    End Property

    Public Property DenyDateRangeFlag() As Boolean
        Get
            Return _DenyDateRangeFlag
        End Get
        Set(ByVal value As Boolean)
            _DenyDateRangeFlag = value
        End Set
    End Property

    Public Property StartDate() As String
        Get
            Return _StartDate
        End Get
        Set(ByVal value As String)
            _StartDate = value
        End Set
    End Property

    Public Property EndDate() As String
        Get
            Return _EndDate
        End Get
        Set(ByVal value As String)
            _EndDate = value
        End Set
    End Property

    Public Property DaysOutFlag() As Boolean
        Get
            Return _DaysOutFlag
        End Get
        Set(ByVal value As Boolean)
            _DaysOutFlag = value
        End Set
    End Property

    Public Property MinDaysOut() As Integer
        Get
            Return _MinDaysOut
        End Get
        Set(ByVal value As Integer)
            _MinDaysOut = value
        End Set
    End Property

    Public Property MaxDaysOut() As Integer
        Get
            Return _MaxDaysOut
        End Get
        Set(ByVal value As Integer)
            _MaxDaysOut = value
        End Set
    End Property

    Public Property SeasonUseFlag() As Boolean
        Get
            Return _SeasonUseFlag
        End Get
        Set(ByVal value As Boolean)
            _SeasonUseFlag = value
        End Set
    End Property

    Public Property SeasonID() As Integer
        Get
            Return _SeasonID
        End Get
        Set(ByVal value As Integer)
            _SeasonID = value
        End Set
    End Property

    Public Property SeasonDenyFlag() As Boolean
        Get
            Return _SeasonDenyFlag
        End Get
        Set(ByVal value As Boolean)
            _SeasonDenyFlag = value
        End Set
    End Property

    Public Property DenyAll() As Boolean
        Get
            Return _DenyAll
        End Get
        Set(ByVal value As Boolean)
            _DenyAll = value
        End Set
    End Property

    Public Property UsageRestrictionID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class

