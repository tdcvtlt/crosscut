Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System


Public Class clsAddress
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _ActiveFlag As Boolean = False
    Dim _Address1 As String = ""
    Dim _Address2 As String = ""
    Dim _City As String = ""
    Dim _StateID As Integer = 0
    Dim _PostalCode As String = ""
    Dim _Region As String = ""
    Dim _CountryID As Integer = 0
    Dim _TypeID As Integer = 0
    Dim _ContractAddress As Boolean = False
    Dim _Err As String = ""
    Dim _UserID As Integer = 0

    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dRead As SqlDataReader

    Public Sub Load()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm = New SqlCommand("Select * from t_ProspectAddress where addressid = " & _ID, cn)
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Address")
            If ds.Tables("Address").Rows.Count > 0 Then
                dr = ds.Tables("Address").Rows(0)
                Set_Values()
            End If
            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            cm = Nothing
            da = Nothing
            ds = Nothing
            dr = Nothing
        End Try
    End Sub

    Public Sub Save()
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm = New SqlCommand("Select * from t_ProspectAddress where AddressID = " & _ID, cn)
            da = New SqlDataAdapter(cm)
            Dim sqlcmdbuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "Address")
            If ds.Tables("Address").Rows.Count = 0 Then
                dr = ds.Tables("Address").NewRow
            Else
                dr = ds.Tables("Address").Rows(0)
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("ActiveFlag", _ActiveFlag, dr)
            Update_Field("Address1", _Address1, dr)
            Update_Field("Address2", _Address2, dr)
            Update_Field("City", _City, dr)
            Update_Field("StateID", _StateID, dr)
            Update_Field("PostalCode", _PostalCode, dr)
            Update_Field("Region", _Region, dr)
            Update_Field("CountryID", _CountryID, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("ContractAddress", _ContractAddress, dr)

            If ds.Tables("Address").Rows.Count < 1 Then ds.Tables("Address").Rows.Add(dr)
            da.Update(ds, "Address")

            If cn.State <> ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        _ID = dr("AddressID")
        _ProspectID = IIf(Not (dr("ProspectID") Is System.DBNull.Value), dr("ProspectID"), 0)
        _ActiveFlag = IIf(Not (dr("ActiveFlag") Is System.DBNull.Value), dr("ActiveFlag"), False)
        _Address1 = IIf(Not (dr("Address1") Is System.DBNull.Value), dr("Address1"), "")
        _Address2 = IIf(Not (dr("Address2") Is System.DBNull.Value), dr("Address2"), "")
        _City = IIf(Not (dr("City") Is System.DBNull.Value), dr("City"), "")
        _StateID = IIf(Not (dr("StateID") Is System.DBNull.Value), dr("StateID"), 0)
        _PostalCode = IIf(Not (dr("PostalCode") Is System.DBNull.Value), dr("PostalCode"), "")
        _Region = IIf(Not (dr("Region") Is System.DBNull.Value), dr("Region"), "")
        _CountryID = IIf(Not (dr("CountryID") Is System.DBNull.Value), dr("CountryID"), 0)
        _TypeID = IIf(Not (dr("TypeID") Is System.DBNull.Value), dr("TypeID"), 0)
        _ContractAddress = IIf(Not (dr("ContractAddress") Is System.DBNull.Value), dr("ContractAddress"), "")
    End Sub

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.SelectCommand = "select AddressID as ID, Address1, Address2, City, s.comboitem as State, PostalCode as Zip, c.comboitem as Country, p.ActiveFlag from t_ProspectAddress p left outer join t_comboitems s on s.comboitemid = p.stateid left outer join t_comboitems c on c.comboitemid = p.CountryID where prospectid = " & _ProspectID & " order by ActiveFlag desc, AddressID asc"
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try

        Return ds
    End Function

    Public Function Get_Table() As DataTable
        Dim dt As New DataTable
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("select p.AddressID as ID,ActiveFlag as Active, Address1, Address2, City,StateID, s.comboitem as State, PostalCode as Zip, Region,CountryID, c.comboitem as Country,ContractAddress as Contract, TypeID, type.comboitem as Type from t_ProspectAddress p left outer join t_comboitems s on s.comboitemid = p.stateid left outer join t_comboitems c on c.comboitemid = p.CountryID left outer join t_Comboitems type on type.comboitemid= p.typeid where prospectid > 0 and prospectid = " & _ProspectID, cn)
        Dim dr As SqlDataReader
        Dim row As DataRow
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
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try

        Return dt
    End Function
    Public Function Get_Address_ID(ByVal prosID As Integer) As Integer
        Dim addID As Integer = 0
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select Top 1 AddressID from t_ProspectAddress where ActiveFlag = 1 and ProspectID = " & prosID, cn)
        Dim dr As SqlDataReader

        Try
            cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                addID = dr("AddressID")
            End If
            dr.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try
        Return addID
    End Function
    Public Function Get_Receipt_Address(ByVal prosID As Integer) As String
        Dim address As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            Dim cm As New SqlCommand("Select top 1 pa.AddressID, pa.Address1, pa.City, st.ComboItem as State, pa.PostalCode from t_ProspectAddress pa left outer join t_ComboItems st on pa.StateID = st.ComboitemID where pa.ActiveFlag = '1' and pa.prospectid = '" & prosID & "' order By pa.ContractAddress desc", cn)
            'cm.CommandText = "Select top 1 pa.AddressID, pa.Address1, pa.City, st.ComboItem as State, pa.PostalCode from t_ProsectAddress pa left outer join t_ComboItems st on pa.StateID = st.ComboitemID where pa.ActiveFlag = '1' and pa.prospectid = '" & prosID & "' orderBy pa.ContractAddress desc"
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                dRead.Read()
                address = "<tr><td>" & dRead("Address1") & "</td></tr>"
                address = address & "<tr><td>" & dRead("City") & ", " & dRead("State") & " " & dRead("PostalCode") & "</td></tr>"
            Else
                address = "<tr><td></td></tr><tr><td></td></tr>"
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cm = Nothing
        End Try
        Return address
    End Function
    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            If sField <> "ProspectID" Then
                oEvents.EventType = "Change"
                oEvents.FieldName = sField
                oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
                oEvents.NewValue = sValue
                oEvents.DateCreated = Date.Now
                oEvents.CreatedByID = _UserID
                oEvents.KeyField = "ProspectID"
                oEvents.KeyValue = _ProspectID
                oEvents.Create_Event()
                oEvents.EventType = "Change"
                oEvents.FieldName = sField
                oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
                oEvents.NewValue = sValue
                oEvents.DateCreated = Date.Now
                oEvents.CreatedByID = _UserID
                oEvents.KeyField = "AddressID"
                oEvents.KeyValue = _ID
                oEvents.Create_Event()
            End If
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
            oEvents = Nothing
    End Sub

    Public Sub Notify_Address_Change_By_Owner(newAddress As clsAddress, emails As List(Of String))

        Using cn = New SqlConnection(Resources.Resource.cns)

            Dim sq = String.Format("select p.ProspectID, c.ContractNumber, p.FirstName + ' ' + p.LastName [Owner] from t_SalesInventory2ContractHist si2ch " _
                    & "inner join t_SalesInventory si on si2ch.SalesInventoryID = si.SalesInventoryID " _
                    & "inner join t_Contract c on si2ch.ContractID = c.ContractID " _
                    & "inner join t_ComboItems cs on cs.ComboItemID = c.StatusID " _
                    & "inner join t_Prospect p on c.ProspectID = p.ProspectID " _
                    & "where cs.ComboItem in ('active', 'on hold') and c.ProspectID = {0} order by cs.ComboItem", ProspectID)

            Using cm = New SqlCommand(sq, cn)

                Try
                    cn.Open()
                    Dim dt = New DataTable
                    dt.Load(cm.ExecuteReader())

                    If dt.Rows.Count > 0 Then

                        Dim sb = New StringBuilder
                        sb.AppendFormat("Owner's Name: {0}", dt.Rows(0)("Owner").ToString())
                        sb.AppendLine()
                        sb.AppendFormat("Prospect ID: {0}", dt.Rows(0)("ProspectID").ToString())
                        sb.AppendLine()
                        sb.AppendFormat("Contract Number: {0}", dt.Rows(0)("ContractNumber").ToString())
                        sb.AppendLine()
                        sb.AppendFormat("Address 1: {0}", newAddress.Address1.Trim())
                        sb.AppendLine()
                        sb.AppendFormat("Address 2: {0}", newAddress.Address2.Trim())
                        sb.AppendLine()
                        sb.AppendFormat("City: {0}", newAddress.City)
                        sb.AppendLine()
                        sb.AppendFormat("State: {0}", New clsComboItems().Lookup_ComboItem(newAddress.StateID))
                        sb.AppendLine()
                        sb.AppendFormat("Postal Code: {0}", newAddress.PostalCode.Trim())

                        For Each email In emails
                            Send_Mail(email, "mis@kingscreekplantation.com", "Address Changes By Owner", sb.ToString(), False)
                        Next
                    End If

                Catch ex As Exception
                    cn.Close()
                    Throw ex
                End Try
            End Using
        End Using
    End Sub

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)

    End Sub

    Protected Overrides Sub Finalize()
        'If cn.State <> ConnectionState.Closed Then cn.Close()
        cm = Nothing
        da = Nothing
        'ds = Nothing
        'dr = Nothing
        'dRead = Nothing

        MyBase.Finalize()
    End Sub

    Public Function Retrieve_Address(ByVal prosID As Integer) As Integer
        Dim addID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            Dim cm As New SqlCommand("Select Top 1 AddressID from t_ProspectAddress where prospectID = " & prosID & " and activeFlag = '1'", cn)
            dRead = cm.ExecuteReader()
            If dRead.HasRows Then
                dRead.Read()
                addID = dRead("AddressID")
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
            addID = -1
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return addID
    End Function

    Public Function Get_Contract_Address(ByVal prosID As Integer) As Integer
        Dim addID As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            Dim cm As New SqlCommand("Select AddressID from t_ProspectAddress where prospectid = '" & prosID & "' order by activeflag desc, contractaddress desc, addressid desc", cn)
            dRead = cm.ExecuteReader
            If dRead.HasRows Then
                dRead.Read()
                addID = dRead("AddressID")
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return addID
    End Function

    Public Function Verify_Address_Type(ByVal AddressID As Integer, ByVal prosID As Integer, ByVal typeID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Addresses from t_ProspectAddress where AddressID <> " & AddressID & " and prospectID = " & prosID & " and TypeID = " & typeID & " and activeflag = 1"
            dRead = cm.ExecuteReader
            dRead.Read()
            If dRead("Addresses") > 0 Then
                bValid = False
            End If
            dRead.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Function Verify_Contract_Address(ByVal addressID As Integer, ByVal prosID As Integer) As Boolean
        Dim bValid As Boolean = True
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Count(*) as Addresses from t_ProspectAddress where AddressID <> " & addressID & " and ProspectID = " & prosID & " and contractAddress = 1"
            dRead = cm.ExecuteReader
            dRead.Read()
            If dRead("Addresses") > 0 Then
                bValid = False
            End If
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return bValid
    End Function

    Public Property AddressID() As Integer
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

    Public Property ActiveFlag() As Boolean
        Get
            Return _ActiveFlag
        End Get

        Set(ByVal value As Boolean)
            _ActiveFlag = value
        End Set
    End Property

    Public Property Address1() As String
        Get
            Return _Address1
        End Get

        Set(ByVal value As String)
            _Address1 = value
        End Set
    End Property

    Public Property Address2() As String
        Get
            Return _Address2
        End Get

        Set(ByVal value As String)
            _Address2 = value
        End Set
    End Property

    Public Property City() As String
        Get
            Return _City
        End Get

        Set(ByVal value As String)
            _City = value
        End Set
    End Property

    Public Property StateID() As Integer
        Get
            Return _StateID
        End Get

        Set(ByVal value As Integer)
            _StateID = value
        End Set
    End Property

    Public Property PostalCode() As String
        Get
            Return _PostalCode
        End Get

        Set(ByVal value As String)
            _PostalCode = value
        End Set
    End Property

    Public Property Region() As String
        Get
            Return _Region
        End Get

        Set(ByVal value As String)
            _Region = value
        End Set
    End Property

    Public Property CountryID() As Integer
        Get
            Return _CountryID
        End Get

        Set(ByVal value As Integer)
            _CountryID = value
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

    Public Property ContractAddress() As Boolean
        Get
            Return _ContractAddress
        End Get

        Set(ByVal value As Boolean)
            _ContractAddress = value
        End Set
    End Property

    Public ReadOnly Property Error_Message() As String
        Get
            Return _Err
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
End Class
