Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsUsageRestriction2Contract
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _UsageRestrictionID As Integer = 0
    Dim _DateCreated As String = ""
    Dim _PersonnelID As Integer = 0
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_UsageRestriction2Contract where UsageRestriction2Contract = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_UsageRestriction2Contract where UsageRestriction2Contract = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_UsageRestriction2Contract")
            If ds.Tables("t_UsageRestriction2Contract").Rows.Count > 0 Then
                dr = ds.Tables("t_UsageRestriction2Contract").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("UsageRestrictionID") Is System.DBNull.Value) Then _UsageRestrictionID = dr("UsageRestrictionID")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("PersonnelID") Is System.DBNull.Value) Then _PersonnelID = dr("PersonnelID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_UsageRestriction2Contract where UsageRestriction2ContractID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_UsageRestriction2Contract")
            If ds.Tables("t_UsageRestriction2Contract").Rows.Count > 0 Then
                dr = ds.Tables("t_UsageRestriction2Contract").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_UsageRestriction2ContractInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@UsageRestrictionID", SqlDbType.int, 0, "UsageRestrictionID")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@UsageRestriction2ContractID", SqlDbType.Int, 0, "UsageRestriction2ContractID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_UsageRestriction2Contract").NewRow
            End If
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("UsageRestrictionID", _UsageRestrictionID, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("PersonnelID", _PersonnelID, dr)
            If ds.Tables("t_UsageRestriction2Contract").Rows.Count < 1 Then ds.Tables("t_UsageRestriction2Contract").Rows.Add(dr)
            da.Update(ds, "t_UsageRestriction2Contract")
            _ID = ds.Tables("t_UsageRestriction2Contract").Rows(0).Item("UsageRestriction2ContractID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Public Function Check_Restrictions(ByVal conID As Integer, ByVal inDate As Date, ByVal outDate As Date, ByVal usageYr As Integer) As Boolean
        Dim bRestricted As Boolean = False
        Dim dRead As SqlDataReader
        Try
            cm.CommandText = "Select * from t_UsageRestriction2Contract urc inner join t_UsageRestriction ur on urc.UsageRestrictionID = ur.usageRestrictionID where urc.ContractID = '" & conID & "'"
            If cn.State <> ConnectionState.Open Then cn.Open()
            dRead = cm.ExecuteReader
            Do While dRead.Read()
                If dRead("AllowDateRangeFlag") Then
                    If Date.Compare(inDate, CDate(dRead("StartDate"))) < 0 Or Date.Compare(outDate, CDate(dRead("EndDate"))) > 0 Then
                        bRestricted = True
                        Exit Do
                    End If
                ElseIf dRead("DenyDateRangeFlag") Then
                    If (Date.Compare(inDate, CDate(dRead("StartDate"))) >= 0 And Date.Compare(inDate, CDate(dRead("EndDate"))) < 0) Or (Date.Compare(outDate, CDate(dRead("EndDate"))) <= 0 And DateTime.Compare(outDate, CDate(dRead("StartDate"))) > 0) Or (usageYr = Year(CDate(dRead("StartDate"))) Or usageYr = Year(CDate(dRead("EndDate")))) Then
                        bRestricted = True
                        Exit Do
                    End If
                ElseIf dRead("DenyAll") Then
                    bRestricted = True
                    Exit Do
                End If
            Loop
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRestricted
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
            oEvents.KeyField = "UsageRestriction2Contract"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
    End Sub
    Public Function List(ByVal conID As Integer) As SQLDataSource
        Dim ds As New SQLDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select urc.UsageRestriction2ContractID, ur.Name, ur.MinStayFlag, ur.MinStay, ur.AllowDateRangeFlag, ur.DenyDateRangeFlag, ur.StartDate, ur.EndDate, ur.daysOutFlag, ur.MinDaysOut, ur.MaxDaysOut, ur.SeasonUseFlag, ur.SeasonID, ur.SeasonDenyFlag, ur.DenyAll, urc.DateCreated, ur.UsageRestrictionID from t_UsageRestriction2Contract urc inner join t_UsageRestriction ur on urc.UsageRestrictionID = ur.UsageRestrictionID where urc.ContractID = '" & conID & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Remove_Restrictor(ByVal ID As Integer) As Boolean
        Dim bDeleted As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_UsageRestriction2Contract where usageRestriction2ContractID = '" & ID & "'"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
            bDeleted = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bDeleted
    End Function
    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
        End Set
    End Property

    Public Property UsageRestrictionID() As Integer
        Get
            Return _UsageRestrictionID
        End Get
        Set(ByVal value As Integer)
            _UsageRestrictionID = value
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

    Public Property PersonnelID() As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
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
    Public Property UsageRestriction2ContractID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
