Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System

Public Class clsPersonnelTrans
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader
    Dim _ID As Integer = 0
    Dim _PersonnelID As Integer = 0
    Dim _TitleID As Integer = 0
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _FixedAmount As Decimal = 0
    Dim _Percentage As Decimal = 0
    Dim _DateCreated As String = ""
    Dim _DatePosted As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _PostedByID As Integer = 0
    Dim _sErr As String = ""
    Dim _UserID As Integer = 0

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Replace", cn)
    End Sub

    Public Sub Load()
        Try
            cn.Open()
            cm.CommandText = "Select PersonnelTransID, PersonnelID, TitleID, KeyField, KeyValue, FixedAmount, Percentage, DateCreated, DatePosted, CreatedByID, PostedByID from t_PersonnelTrans where PersonnelTransID =" & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "PT")
            If ds.Tables("PT").Rows.Count > 0 Then
                dr = ds.Tables("PT").Rows(0)
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
                ds.SelectCommand = "Select pt.PersonnelTransID as ID, p.LastName, p.FirstName,t.Comboitem as Title from t_PersonnelTrans pt inner join t_Personnel p on p.personnelid = pt.personnelid left outer join t_Comboitems t on t.comboitemid = pt.titleid  where pt.Keyfield='" & _KeyField & "' and pt.Keyvalue = " & _KeyValue
            Else
                ds.SelectCommand = "Select pt.PersonnelTransID as ID, p.LastName, p.FirstName,t.Comboitem as Title from t_PersonnelTrans pt inner join t_Personnel p on p.personnelid = pt.personnelid left outer join t_Comboitems t on t.comboitemid = pt.titleid  where 1=2"
            End If
        Catch ex As Exception
            _sErr = ex.ToString
        End Try
        Return ds
    End Function

    Public Function Get_Table() As DataTable
        Dim dt As New DataTable
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select pt.PersonnelTransID as ID, p.personnelid, p.LastName, p.FirstName,t.comboitemid as TitleID, t.Comboitem as Title, pt.Percentage, pt.FixedAmount from t_PersonnelTrans pt inner join t_Personnel p on p.personnelid = pt.personnelid left outer join t_Comboitems t on t.comboitemid = pt.titleid  where 1=2", cn)
        Dim dr As SqlDataReader

        Dim i As Integer
        Try
            cn.Open()
            dr = cm.ExecuteReader
            dr.Read()
            For i = 0 To dr.VisibleFieldCount - 1
                dt.Columns.Add(dr.GetName(i))
            Next
            dt.Columns.Add("Dirty")

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

    Public Function Get_Con_Personnel(ByVal conID As Integer) As DataTable
        Dim dt As New DataTable
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select pt.PersonnelTransID as ID, p.personnelid, p.LastName, p.FirstName,t.comboitemid as TitleID, t.Comboitem as Title, pt.Percentage, pt.FixedAmount from t_PersonnelTrans pt inner join t_Personnel p on p.personnelid = pt.personnelid left outer join t_Comboitems t on t.comboitemid = pt.titleid  where keyfield = 'ContractID' and keyvalue = " & conID, cn)
        Dim dr As SqlDataReader

        Dim i As Integer
        Try
            cn.Open()
            dr = cm.ExecuteReader
            dr.Read()
            For i = 0 To dr.VisibleFieldCount - 1
                dt.Columns.Add(dr.GetName(i))
            Next
            dt.Columns.Add("Dirty")

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

    Public Function Move_Trans(ByVal oldID As Integer, ByVal newID As Integer, ByVal keyField As String) As Boolean
        Dim bMoved As Boolean = True
        Try
            Dim oPersTrans As New clsPersonnelTrans
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cm.CommandText = "Select * from t_PersonnelTrans where keyField = '" & keyField & "' and keyvalue = " & oldID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    oPersTrans.Personnel_Trans_ID = 0
                    oPersTrans.Load()
                    oPersTrans.KeyValue = newID
                    oPersTrans.KeyField = keyField
                    oPersTrans.PersonnelID = dread("PersonnelID")
                    oPersTrans.TitleID = dread("TitleID")
                    oPersTrans.Fixed_Amount = dread("FixedAmount")
                    oPersTrans.Percentage = dread("Percentage")
                    oPersTrans.Date_Created = dread("DateCreated")
                    oPersTrans.Created_By_ID = _CreatedByID
                    oPersTrans.Save()
                Loop
            End If
            dread.Close()
            oPersTrans = Nothing
        Catch ex As Exception
            _sErr = ex.Message
            bMoved = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bMoved
    End Function

    Private Sub Fill_Values()
        Try
            _ID = dr("PersonnelTransID")
            _PersonnelID = IIf(IsNumeric(dr("PersonnelID")), dr("PersonnelID"), 0)
            _TitleID = IIf(IsNumeric(dr("TitleID")), dr("TitleID"), 0)
            _KeyField = IIf(dr("KeyField") Is System.DBNull.Value, "", dr("KeyField"))
            _KeyValue = IIf(IsNumeric(dr("KeyValue")), dr("KeyValue"), 0)
            _FixedAmount = IIf(IsNumeric(dr("FixedAmount")), dr("FixedAmount"), 0)
            _Percentage = IIf(IsNumeric(dr("Percentage")), dr("Percentage"), 0)
            _DateCreated = IIf(dr("DateCreated") Is System.DBNull.Value, "", dr("DateCreated"))
            _DatePosted = IIf(dr("DatePosted") Is System.DBNull.Value, "", dr("DatePosted"))
            _CreatedByID = IIf(IsNumeric(dr("CreatedByID")), dr("CreatedByID"), 0)
            _PostedByID = IIf(IsNumeric(dr("PostedByID")), dr("PostedByID"), 0)
        Catch ex As Exception
            _sErr = ex.ToString
        End Try
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PersonnelTrans where PersonnelTransID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PersonnelTrans")
            If ds.Tables("t_PersonnelTrans").Rows.Count > 0 Then
                dr = ds.Tables("t_PersonnelTrans").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PersonnelTransInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PersonnelID", SqlDbType.int, 0, "PersonnelID")
                da.InsertCommand.Parameters.Add("@TitleID", SqlDbType.int, 0, "TitleID")
                da.InsertCommand.Parameters.Add("@KeyField", SqlDbType.varchar, 0, "KeyField")
                da.InsertCommand.Parameters.Add("@KeyValue", SqlDbType.int, 0, "KeyValue")
                da.InsertCommand.Parameters.Add("@FixedAmount", SqlDbType.money, 0, "FixedAmount")
                da.InsertCommand.Parameters.Add("@Percentage", SqlDbType.Money, 0, "Percentage")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.datetime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@DatePosted", SqlDbType.datetime, 0, "DatePosted")
                da.InsertCommand.Parameters.Add("@CreatedByID", SqlDbType.int, 0, "CreatedByID")
                da.InsertCommand.Parameters.Add("@PostedByID", SqlDbType.int, 0, "PostedByID")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.int, 0, "CRMSID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@PersonnelTransID", SqlDbType.Int, 0, "PersonnelTransID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PersonnelTrans").NewRow
            End If
            Update_Field("PersonnelID", _PersonnelID, dr)
            Update_Field("TitleID", _TitleID, dr)
            Update_Field("KeyField", _KeyField, dr)
            Update_Field("KeyValue", _KeyValue, dr)
            Update_Field("FixedAmount", _FixedAmount, dr)
            Update_Field("Percentage", _Percentage, dr)
            _DateCreated = IIf(_DateCreated = "", Date.Now, _DateCreated)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("DatePosted", _DatePosted, dr)
            Update_Field("CreatedByID", _CreatedByID, dr)
            Update_Field("PostedByID", _PostedByID, dr)
            'Update_Field("CRMSID", _CRMSID, dr)
            If ds.Tables("t_PersonnelTrans").Rows.Count < 1 Then ds.Tables("t_PersonnelTrans").Rows.Add(dr)
            da.Update(ds, "t_PersonnelTrans")
            _ID = ds.Tables("t_PersonnelTrans").Rows(0).Item("PersonnelTransID")
            Return True
        Catch ex As Exception
            _sErr = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    'Public Sub Save()
    '    cm.CommandText = "Select * from t_PersonnelTrans where PersonnelTransID = " & _ID
    '    da = New SqlDataAdapter(cm)
    '    ds = New DataSet
    '    Dim sqlCMD As New SqlCommandBuilder(da)
    '    Try
    '        If cn.State <> ConnectionState.Open Then cn.Open()
    '        da.Fill(ds, "PT")
    '        If ds.Tables("PT").Rows.Count > 0 Then
    '            dr = ds.Tables("PT").Rows(0)
    '        Else
    '            dr = ds.Tables("PT").NewRow
    '        End If
    '        Update_Field("PersonnelID", _PersonnelID, dr) ' dr("PersonnelID") = _PersonnelID
    '        Update_Field("TitleID", _TitleID, dr) 'dr("TitleID") = _TitleID
    '        Update_Field("KeyField", _KeyField, dr) 'dr("KeyField") = _KeyField
    '        Update_Field("KeyValue", _KeyValue, dr) 'dr("KeyValue") = _KeyValue
    '        Update_Field("FixedAmount", _FixedAmount, dr) 'dr("FixedAmount") = _FixedAmount
    '        Update_Field("Percentage", _Percentage, dr) 'dr("Percentage") = _Percentage
    '        If _DateCreated = "" Then
    '            Update_Field("DateCreated", Date.Now, dr) 'dr("DateCreated") = Date.Now
    '        End If
    '        If _DatePosted <> "" Then Update_Field("DatePosted", _DatePosted, dr) 'dr("DatePosted") = _DatePosted
    '        Update_Field("CreatedByID", _CreatedByID, dr) 'dr("CreatedByID") = _CreatedByID
    '        Update_Field("PostedByID", _PostedByID, dr) 'dr("PostedByID") = _PostedByID

    '        If ds.Tables("PT").Rows.Count <= 0 Then ds.Tables("PT").Rows.Add(dr)

    '        da.Update(ds, "PT")

    '    Catch ex As Exception
    '        _sErr = ex.ToString
    '    Finally
    '        If cn.State <> ConnectionState.Closed Then cn.Close()
    '        sqlCMD = Nothing

    '    End Try
    'End Sub

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = System.DateTime.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = _KeyField
            oEvents.KeyValue = _KeyValue
            oEvents.Create_Event()
            drow(sField) = sValue
            '_Err = oEvents.Error_Message
        End If
        oEvents = Nothing
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

    Public Function Remove_Item(ByVal ID As Integer) As Boolean
        Dim bRemoved As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Delete from t_PersonnelTrans where personneltransid = " & ID
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _sErr = ex.Message
            bRemoved = False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bRemoved
    End Function

    Public ReadOnly Property Error_Message() As String
        Get
            Return _sErr
        End Get
    End Property

    Public Property Personnel_Trans_ID As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property PersonnelID As Integer
        Get
            Return _PersonnelID
        End Get
        Set(ByVal value As Integer)
            _PersonnelID = value
        End Set
    End Property

    Public Property TitleID() As Integer
        Get
            Return _TitleID
        End Get
        Set(ByVal value As Integer)
            _TitleID = value
        End Set
    End Property

    Public Property KeyField As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
        End Set
    End Property

    Public Property KeyValue As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
        End Set
    End Property

    Public Property Fixed_Amount As Decimal
        Get
            Return _FixedAmount
        End Get
        Set(ByVal value As Decimal)
            _FixedAmount = value
        End Set
    End Property

    Public Property Percentage As Decimal
        Get
            Return _Percentage
        End Get
        Set(ByVal value As Decimal)
            _Percentage = value
        End Set
    End Property

    Public Property Date_Created As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property Date_Posted As String
        Get
            Return _DatePosted
        End Get
        Set(ByVal value As String)
            _DatePosted = value
        End Set
    End Property

    Public Property Created_By_ID As Integer
        Get
            Return _CreatedByID
        End Get
        Set(ByVal value As Integer)
            _CreatedByID = value
        End Set
    End Property

    Public Property Posted_By_ID As Integer
        Get
            Return _PostedByID
        End Get
        Set(ByVal value As Integer)
            _PostedByID = value
        End Set
    End Property

    Public Property UserID As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property
End Class
