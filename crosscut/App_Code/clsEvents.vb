Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System


Public Class clsEvents
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader
    Dim _ID As Integer = 0
    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _EventType As String = ""
    Dim _EventSubType As String = ""
    Dim _OldValue As String = ""
    Dim _NewValue As String = ""
    Dim _DateCreated As String = ""
    Dim _CreatedByID As Integer = 0
    Dim _CreatedBy As String = ""
    Dim _FieldName As String = ""
    Dim _Err As String = ""

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Replace", cn)
    End Sub

    Public Sub Load()
        Try
            If _ID > 0 Then
                cn.Open()
                cm.CommandText = "Select e.EventID,e.FieldName, e.Type, e.SubType,e.OldValue, e.NewValue, e.DateCreated,p.username as CreateBy from t_Event e left outer join t_Personnel p on p.personnelid = e.createdbyid where EventID = " & _ID
                dread = cm.ExecuteReader
                If dread.HasRows Then
                    Fill_Values()
                End If
                cn.Close()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            If _KeyValue > 0 Then
                ds.SelectCommand = "Select e.EventID,e.FieldName, e.Type,e.SubType, case when e.FieldName like '%SSN' then 'XXX-XX-XXXX' else e.OldValue end as OldValue, case when e.FieldName like '%SSN' then 'XXX-XX-XXXX' else e.NewValue end as NewValue, e.DateCreated,p.username as CreateBy from t_Event e left outer join t_Personnel p on p.personnelid = e.createdbyid where KeyField='" & _KeyField & "' and KeyValue = " & _KeyValue & " order by eventid desc"
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function Find_View_Event(ByVal keyfield As String, ByVal keyvalue As Integer, ByVal mins As Integer, ByVal userid As Integer, ByRef sErr As String) As Boolean
        Try
            cn.Open()
            cm.CommandText = "select * from t_Event where keyfield = '" & keyfield & "' and keyvalue = " & keyvalue & " and type='view' and datediff(mi, datecreated, '" & Date.Now & "') < " & mins & " and createdbyid = " & userid
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            sErr = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            dread.Close()
        End Try
    End Function

    Public Function Create_View_Event(ByVal keyfield As String, ByVal keyvalue As Integer, ByVal mins As Integer, ByVal userid As Integer, ByRef sErr As String) As Boolean
        Try
            cn.Open()
            cm.CommandText = "Insert into t_Event (KeyField, KeyValue,Type,SubType, OldValue, NewValue,DateCreated,CreatedByID,FieldName) values ('" & keyfield & "'," & keyvalue & ",'View','','','','" & Date.Now & "'," & userid & ",'')"
            cm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sErr = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    Public Function Create_Create_Event(ByVal keyfield As String, ByVal keyvalue As Integer, ByVal mins As Integer, ByVal userid As Integer, ByRef sErr As String) As Boolean
        Try
            cn.Open()
            cm.CommandText = "Insert into t_Event (KeyField, KeyValue,Type,SubType, OldValue, NewValue,DateCreated,CreatedByID,FieldName) values ('" & keyfield & "'," & keyvalue & ",'Create','','','','" & Date.Now & "'," & userid & ",'')"
            cm.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            sErr = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    Private Sub Fill_Values()
        _ID = dr("EventID")
        _EventType = dr("Type")
        _EventSubType = dr("SubType")
        _OldValue = dr("OldValue")
        _NewValue = dr("NewValue")
        _DateCreated = dr("DateCreated")
        _CreatedByID = dr("CreatedByID")
        _CreatedBy = dr("CreatedBy")
        _FieldName = dr("FieldName")
    End Sub

    Public Sub Create_Event()
        Try
            If _KeyField <> "" And _KeyValue > 0 And _EventType <> "" Then
                If cn.State <> ConnectionState.Open Then cn.Open()
                If UCase(Right(_FieldName, 2) = "ID") _
                    And LCase(_FieldName) <> "prospectid" _
                    And LCase(_FieldName) <> "contractid" _
                    And LCase(_FieldName) <> "mortgageid" _
                    And LCase(_FieldName) <> "usageid" _
                    And LCase(_FieldName) <> "reservationid" _
                    And LCase(_FieldName) <> "roomid" _
                    And LCase(_FieldName) <> "unitid" _
                    And LCase(_FieldName) <> "tourid" _
                    And LCase(_FieldName) <> "salesinventoryid" _
                    And LCase(_FieldName) <> "personnelid" _
                    And LCase(_FieldName) <> "soldinventoryid" _
                    And LCase(_FieldName) <> "frequencyid" _
                    And LCase(_FieldName) <> "campaignid" _
                    And LCase(_FieldName) <> "locationid" _
                    And LCase(_FieldName) <> "vendorid" _
                    And LCase(_FieldName) <> "spouseid" _
                    And LCase(_FieldName) <> "soldinventoryid" _
                    And LCase(_FieldName) <> "referringprospectid" _
                    And LCase(_FieldName) <> "referrerid" _
                    And LCase(_FieldName) <> "punchid" _
                    And LCase(_FieldName) <> "premiumid" _
                    And LCase(_FieldName) <> "postedbyid" _
                    And LCase(_FieldName) <> "paymentid" _
                    And LCase(_FieldName) <> "packageissuedid" _
                    And LCase(_FieldName) <> "packageid" _
                    And LCase(_FieldName) <> "outuserid" _
                    And LCase(_FieldName) <> "outclockid" _
                    And LCase(_FieldName) <> "noteid" _
                    And LCase(_FieldName) <> "merchantaccountid" _
                    And LCase(_FieldName) <> "managerid" _
                    And LCase(_FieldName) <> "lockoutid" _
                    And LCase(_FieldName) <> "linkedmissedpunchid" _
                    And LCase(_FieldName) <> "issuedid" _
                    And LCase(_FieldName) <> "issuedbyid" _
                    And LCase(_FieldName) <> "inuserid" _
                    And LCase(_FieldName) <> "installedbyid" _
                    And LCase(_FieldName) <> "importedid" _
                    And LCase(_FieldName) <> "hrid" _
                    And LCase(_FieldName) <> "fintransid" _
                    And LCase(_FieldName) <> "dpinvoiceid" _
                    And LCase(_FieldName) <> "depositedbyid" _
                    And LCase(_FieldName) <> "crmsid" _
                    And LCase(_FieldName) <> "createdbyid" _
                    And LCase(_FieldName) <> "ccinvoiceid" _
                    And LCase(_FieldName) <> "adpid" _
                    And LCase(_FieldName) <> "approvedbyid" _
                    And LCase(_FieldName) <> "assignedtoid" _
                    Then

                    Dim oCI As New clsComboItems
                    If _OldValue <> 0 Then
                        oCI.ID = _OldValue
                        oCI.Load()
                        If oCI.Comboitem <> "" Then _OldValue = oCI.Comboitem Else _OldValue = ""
                    Else
                        _OldValue = ""
                    End If
                    If _NewValue <> 0 Then
                        oCI.ID = _NewValue
                        oCI.Load()
                        If oCI.Comboitem <> "" Then _NewValue = oCI.Comboitem Else _NewValue = ""
                    Else
                        _NewValue = ""
                    End If
                    oCI = Nothing
                End If
                cm.CommandText = "Insert into t_Event (KeyField, KeyValue,Type,SubType, OldValue, NewValue,DateCreated,CreatedByID,FieldName) values ('" & _KeyField & "'," & _KeyValue & ",'" & _EventType & "','" & _EventSubType & "','" & _OldValue & "','" & _NewValue & "','" & Date.Now & "'," & _CreatedByID & ",'" & _FieldName & "')"
                cm.ExecuteNonQuery()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        'If cn.State <> ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        da = Nothing
        ds = Nothing
        dr = Nothing
        dread = Nothing
        MyBase.Finalize()
    End Sub

    Public ReadOnly Property Error_Message() As String
        Get
            Return _Err
        End Get
    End Property

    Public Property EventID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
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

    Public Property KeyValue() As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
        End Set
    End Property

    Public Property EventType() As String
        Get
            Return _EventType
        End Get
        Set(ByVal value As String)
            _EventType = value
        End Set
    End Property

    Public Property EventSubType() As String
        Get
            Return _EventSubType
        End Get
        Set(ByVal value As String)
            _EventSubType = value
        End Set
    End Property

    Public Property OldValue() As String
        Get
            Return _OldValue
        End Get
        Set(ByVal value As String)
            _OldValue = value
        End Set
    End Property

    Public Property NewValue() As String
        Get
            Return _NewValue
        End Get
        Set(ByVal value As String)
            _NewValue = value
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

    Public Property FieldName() As String
        Get
            Return _FieldName
        End Get
        Set(ByVal value As String)
            _FieldName = value
        End Set
    End Property

End Class
