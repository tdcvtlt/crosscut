Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.Data
Imports System

Public Class clsContract
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _TourID As Integer = 0
    Dim _LocationID As Integer = 0
    Dim _ContractNumber As String = ""
    Dim _ContractDate As String = ""
    Dim _OccupancyDate As String = ""
    Dim _StatusID As Integer = 0
    Dim _SubStatusID As Integer = 0
    Dim _DeededStatusID As Integer = 0
    Dim _DeededDate As String = ""
    Dim _FrequencyID As Integer = 0
    Dim _WeekTypeID As Integer = 0
    Dim _SeasonID As Integer = 0
    Dim _CampaignID As Integer = 0
    Dim _PropertyTaxAmount As Decimal = 0
    Dim _BillingCodeID As Integer = 0
    Dim _SaleTypeID As Integer = 0
    Dim _SaleSubType As Integer = 0
    Dim _FundingStatusID As Integer = 0
    Dim _FundingDate As String = ""
    Dim _TypeID As Integer = 0
    Dim _SubTypeID As Integer = 0
    Dim _TrustName As String = ""
    Dim _TrustFlag As Boolean = False
    Dim _CompanyName As String = ""
    Dim _CompanyFlag As Boolean = False
    Dim _SplitMF As Boolean = False
    Dim _MaintenanceFeeAmount As Decimal = 0
    Dim _StatusDate As String = ""
    Dim _AnniversaryDate As String = ""
    Dim _OriginallyWrittenDate As String = ""
    Dim _MaintenanceFeeCodeID As Integer = 0
    Dim _MaintenanceFeeStatusID As Integer = 0
    Dim _IgnoreStatusDate As Boolean = False
    Dim _Err As String = ""


    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow
    Dim dread As SqlDataReader


    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Contract where ContractID = " & _ID, cn)

    End Sub

    Public Sub Load()
        Try
            If _ID > 0 Then
                cm.CommandText = "Select * from t_Contract where contractid = " & _ID
            ElseIf _ContractNumber <> "" Then
                cm.CommandText = "Select * from t_Contract where contractnumber='" & _ContractNumber & "'"
            Else
                cm.CommandText = "Select * from t_Contract where contractid = " & _ID & " or contractnumber='" & _ContractNumber & "'"
            End If

            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "Contract")

            If ds.Tables("Contract").Rows.Count > 0 Then
                dr = ds.Tables("Contract").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Public Function Check_Collection(ByVal prosID As Integer) As Integer
        Dim ret As Integer = 0
        Try
            cn.Open()
            cm.CommandText = "Select Count(*) as Contracts from t_Contract c inner join t_ComboItems mfs on c.maintenanceFeestatusid = mfs.ComboItemID where mfs.ComboItem = 'In Collections' and c.ProspectID = '" & prosID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ret = dread("Contracts")
            End If
            dread.Close()
            cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ret
    End Function

    Public Function Check_Bankruptcy(ByVal prosID As Integer) As Integer
        Dim ret As Integer = 0
        Try
            cn.Open()
            cm.CommandText = "Select Count(*) as Contracts from t_Contract c inner join t_ComboItems cs on c.StatusID = cs.ComboItemID inner join t_ComboItems css on c.SubStatusID = css.ComboitemID where cs.ComboItem = 'On Hold' and css.ComboItem = 'Bankruptcy' and c.ProspectID = '" & prosID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ret = dread("Contracts")
            End If
            dread.Close()
            cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ret
    End Function

    Public Function Get_Point_Value(ByVal ContID As Integer) As Integer
        Dim ret As Integer = 0
        Try
            cn.Open()
            cm.CommandText = "Select sum(points) as Points from t_Soldinventory where contractid = " & ContID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                ret = dread("Points")
            End If
            dread.Close()
            cn.Close()
        Catch ex As Exception
            _Err = ex.ToString
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return ret
    End Function

    Public Function Query(Optional ByVal iLimit As Integer = 0, Optional ByVal sFilterField As String = "", Optional ByVal sFilterValue As String = "", Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            Dim sName(2) As String
            Dim sql As String = "Select "
            sql += IIf(iLimit > 0, " top " & iLimit.ToString & " ", "")
            sql += " c.ContractID as ID, c.ContractNumber as [KCP Number], cs.comboitem as Status, ss.ComboItem as SubStatus, mf.ComboItem as MFStatus, ms.comboitem as MortgageStatus, cst.ComboItem as SubType, p.LastName, p.FirstName, Case when sp.ProspectID = p.ProspectID then sp.SpouseLastName else sp.LastName end as LastName1, Case when sp.ProspectID = p.ProspectID then sp.SpouseFirstName else sp.FirstName end as FirstName1, p.ProspectID from t_Contract c left outer join t_Prospect p on p.prospectid = c.prospectid left outer join t_ContractCoOwner co on co.contractid = c.contractid left outer join t_Prospect sp on sp.prospectid = co.prospectid left outer join t_Comboitems cs on cs.comboitemid = c.statusid left outer join t_ComboItems ss on c.SubStatusID = ss.ComboitemID left outer join t_ComboItems mf on c.MaintenanceFeeStatusID = mf.CombOitemID left outer join t_Mortgage m on m.contractid = c.contractid left outer join t_Comboitems ms on ms.comboitemid = m.statusid left outer join t_ComboItems cst on c.SubTypeID = cst.ComboItemID "
            If sFilterField <> "" Then
                If sFilterField = "Co-Owner" Then
                    'If InStr(sFilterValue, ",") > 0 And InStr(sFilterValue, ", ") < 1 Then sFilterValue = Replace(sFilterValue, ",", ", ")
                    If InStr(sFilterValue, ",") > 0 Then
                        sName = sFilterValue.Split(",")
                        sql += " where sp.lastname like '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and sp.firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%' "
                    Else
                        sql += " where sp.lastname like '" & sFilterValue.Replace(New Char() {"'"}, "''") & "' "
                    End If
                ElseIf sFilterField = "ContractID" Then
                    sql += " where c.contractid like '" & sFilterValue & "%' "
                ElseIf sFilterField = "Owner" Then
                    'If InStr(sFilterValue, ",") > 0 And InStr(sFilterValue, ", ") < 1 Then sFilterValue = Replace(sFilterValue, ",", ", ")
                    sName = sFilterValue.Split(",")
                    If UBound(sName) = 0 Then
                        sql += " where p.lastname LIKE '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%'"
                    Else
                        sql += " where p.lastname LIKE '" & Trim(sName(0)).Replace(New Char() {"'"}, "''") & "%' and p.firstname like '" & Trim(sName(1)).Replace(New Char() {"'"}, "''") & "%' "
                    End If

                ElseIf sFilterField = "Trust" Then
                    sql += " where c.trustname like '" & sFilterValue.Replace(New Char() {"'"}, "''") & "%' "
                ElseIf sFilterField = "Company" Then
                    'sql += " where p.companyname like '" & sFilterValue.Replace(New Char() {"'"}, "''") & "%' "
                    sql += " where p.companyname like '" & sFilterValue.Replace(New Char() {"'"}, "''") & "%' or c.Companyname like '" & sFilterValue.Replace(New Char() {"'"}, "''") & "%' "
                Else
                    sql += " where " & sFilterField & " like '" & sFilterValue & "%' "
                End If
            End If

            sql += IIf(sOrderBy <> "", " order by " & sOrderBy, "")
            ds.SelectCommand = sql
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function List(Optional ByVal iLimit As Integer = 0, Optional ByVal sFilterField As String = "", Optional ByVal sFilterValue As String = "", Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            Dim sql As String = "Select "
            Sql += IIf(iLimit > 0, " top " & iLimit.ToString & " ", "")
            sql += " c.ContractID, c.ContractNumber as [KCP Number], cs.comboitem as Status from t_Contract c left outer join t_Comboitems cs on cs.comboitemid = c.statusid "
            Sql += IIf(sFilterField <> "", " where " & sFilterField & " = '" & sFilterValue & "' ", "")
            Sql += IIf(sOrderBy <> "", " order by " & sOrderBy, "")
            ds.SelectCommand = Sql
            ds.ConnectionString = Resources.resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Public Function List_OwnContracts(Optional ByVal iLimit As Integer = 0, Optional ByVal sFilterField As String = "", Optional ByVal sFilterValue As String = "", Optional ByVal sOrderBy As String = "") As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            Dim sql As String = "Select "
            sql += " con.ContractID, con.ContractNumber as [KCP Number], css.comboitem as Status,  sss.comboitem [Sub Status], 'Primary' as Ownership, cst.ComboItem as SubType from t_Contract con left outer join t_Comboitems css on css.comboitemid = con.statusid left outer join t_ComboItems cst on con.SubTypeID = cst.Comboitemid left outer join t_ComboItems sss on con.SubStatusID = sss.Comboitemid "
            sql += " where con.ProspectID = '" & sFilterValue & "' "
            sql += " UNION "
            sql += " Select "
            sql += " c.ContractID, c.ContractNumber as [KCP Number], cs.comboitem as Status, sss.comboitem [Sub Status], 'Co-Owner' as Ownership, cst.ComboItem as SubType from t_Contract c left outer join t_Comboitems cs on cs.comboitemid = c.statusid left outer join t_ComboItems cst on c.SubTypeID = cst.Comboitemid left outer join t_ComboItems sss on c.SubStatusID = sss.Comboitemid  "
            sql += " where c.ContractID in (Select contractid from t_ContractCoOwner where prospectID = " & sFilterValue & " and prospectID <> c.ProspectID) "
            sql += " order by ContractID"
            ds.SelectCommand = sql
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception
            _Err = ex.ToString
        End Try
        Return ds
    End Function

    Private Sub Set_Values()
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ID = dr("ContractID")
        If Not (dr("ProspectID") Is System.DBNull.Value) Then _ProspectID = dr("ProspectID")
        If Not (dr("TourID") Is System.DBNull.Value) Then _TourID = dr("TourID")
        If Not (dr("LocationID") Is System.DBNull.Value) Then _LocationID = dr("LocationID")
        If Not (dr("ContractNumber") Is System.DBNull.Value) Then _ContractNumber = dr("ContractNumber")
        If Not (dr("ContractDate") Is System.DBNull.Value) Then _ContractDate = dr("ContractDate")
        If Not (dr("OccupancyDate") Is System.DBNull.Value) Then _OccupancyDate = dr("OccupancyDate")
        If Not (dr("StatusID") Is System.DBNull.Value) Then _StatusID = dr("StatusID")
        If Not (dr("SubStatusID") Is System.DBNull.Value) Then _SubStatusID = dr("SubStatusID")
        If Not (dr("DeededStatusID") Is System.DBNull.Value) Then _DeededStatusID = dr("DeededStatusID")
        If Not (dr("DeededDate") Is System.DBNull.Value) Then _DeededDate = dr("DeededDate")
        If Not (dr("FrequencyID") Is System.DBNull.Value) Then _FrequencyID = dr("FrequencyID")
        If Not (dr("WeekTypeID") Is System.DBNull.Value) Then _WeekTypeID = dr("WeekTypeID")
        If Not (dr("SeasonID") Is System.DBNull.Value) Then _SeasonID = dr("SeasonID")
        If Not (dr("CampaignID") Is System.DBNull.Value) Then _CampaignID = dr("CampaignID")
        If Not (dr("PropertyTaxAmount") Is System.DBNull.Value) Then _PropertyTaxAmount = dr("PropertyTaxAmount")
        If Not (dr("BillingCodeID") Is System.DBNull.Value) Then _BillingCodeID = dr("BillingCodeID")
        If Not (dr("SaleTypeID") Is System.DBNull.Value) Then _SaleTypeID = dr("SaleTypeID")
        If Not (dr("SaleSubTypeID") Is System.DBNull.Value) Then _SaleSubType = dr("SaleSubTypeID")
        If Not (dr("FundingStatusID") Is System.DBNull.Value) Then _FundingStatusID = dr("FundingStatusID")
        If Not (dr("FundingDate") Is System.DBNull.Value) Then _FundingDate = dr("FundingDate")
        If Not (dr("StatusDate") Is System.DBNull.Value) Then _StatusDate = dr("StatusDate")
        If Not (dr("TypeID") Is System.DBNull.Value) Then _TypeID = dr("TypeID")
        If Not (dr("SubTypeID") Is System.DBNull.Value) Then _SubTypeID = dr("SubTypeID")
        If Not (dr("TrustName") Is System.DBNull.Value) Then _TrustName = dr("TrustName")
        If Not (dr("TrustFlag") Is System.DBNull.Value) Then _TrustFlag = dr("TrustFlag")
        If Not (dr("CompanyFlag") Is System.DBNull.Value) Then _CompanyFlag = dr("CompanyFlag")
        If Not (dr("CompanyName") Is System.DBNull.Value) Then _CompanyName = dr("CompanyName")
        If Not (dr("SplitMF") Is System.DBNull.Value) Then _SplitMF = dr("SplitMF")
        If Not (dr("AnniversaryDate") Is System.DBNull.Value) Then _AnniversaryDate = dr("AnniversaryDate")
        If Not (dr("OriginallyWrittenDate") Is System.DBNull.Value) Then _OriginallyWrittenDate = dr("OriginallyWrittenDate")
        If Not (dr("MaintenanceFeeCodeID") Is System.DBNull.Value) Then _MaintenanceFeeCodeID = dr("MaintenanceFeeCodeID")
        If Not (dr("MaintenanceFeeAmount") Is System.DBNull.Value) Then _MaintenanceFeeAmount = dr("MaintenanceFeeAmount")
        If Not (dr("MaintenanceFeeStatusID") Is System.DBNull.Value) Then _MaintenanceFeeStatusID = dr("MaintenanceFeeStatusID")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Contract where contractid = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "Contract")
            If ds.Tables("Contract").Rows.Count > 0 Then

                dr = ds.Tables("Contract").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_ContractInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ProspectID", SqlDbType.Int, 0, "ProspectID")
                da.InsertCommand.Parameters.Add("@TourID", SqlDbType.Int, 0, "TourID")
                da.InsertCommand.Parameters.Add("@LocationID", SqlDbType.Int, 0, "LocationID")
                da.InsertCommand.Parameters.Add("@ContractNumber", SqlDbType.VarChar, 50, "ContractNumber")
                da.InsertCommand.Parameters.Add("@ContractDate", SqlDbType.SmallDateTime, 0, "ContractDate")
                da.InsertCommand.Parameters.Add("@OccupancyDate", SqlDbType.SmallDateTime, 0, "OccupancyDate")
                da.InsertCommand.Parameters.Add("@StatusID", SqlDbType.Int, 0, "StatusID")
                da.InsertCommand.Parameters.Add("@SubStatusID", SqlDbType.Int, 0, "SubStatusID")
                da.InsertCommand.Parameters.Add("@DeededStatusID", SqlDbType.Int, 0, "DeededStatusID")
                da.InsertCommand.Parameters.Add("@DeededDate", SqlDbType.SmallDateTime, 15, "DeededDate")
                da.InsertCommand.Parameters.Add("@FrequencyID", SqlDbType.Int, 0, "FrequencyID")
                da.InsertCommand.Parameters.Add("@WeekTypeID", SqlDbType.Int, 0, "WeekTypeID")
                da.InsertCommand.Parameters.Add("@SeasonID", SqlDbType.Int, 0, "SeasonID")
                da.InsertCommand.Parameters.Add("@CampaignID", SqlDbType.Int, 0, "CampaignID")
                da.InsertCommand.Parameters.Add("@PropertyTaxAmount", SqlDbType.Money, 0, "PropertyTaxAmount")
                da.InsertCommand.Parameters.Add("@BillingCodeID", SqlDbType.Int, 0, "BillingCodeID")
                da.InsertCommand.Parameters.Add("@SaleTypeID", SqlDbType.Int, 0, "SaleTypeID")
                da.InsertCommand.Parameters.Add("@SaleSubTypeID", SqlDbType.Int, 0, "SaleSubTypeID")
                da.InsertCommand.Parameters.Add("@FundingStatusID", SqlDbType.Int, 0, "FundingStatusID")
                da.InsertCommand.Parameters.Add("@FundingDate", SqlDbType.SmallDateTime, 0, "FundingDate")
                da.InsertCommand.Parameters.Add("@StatusDate", SqlDbType.SmallDateTime, 0, "StatusDate")
                da.InsertCommand.Parameters.Add("@TypeID", SqlDbType.Int, 0, "TypeID")
                da.InsertCommand.Parameters.Add("@SubTypeID", SqlDbType.Int, 0, "SubTypeID")
                da.InsertCommand.Parameters.Add("@TrustName", SqlDbType.VarChar, 255, "TrustName")
                da.InsertCommand.Parameters.Add("@TrustFlag", SqlDbType.Bit, 0, "TrustFlag")
                da.InsertCommand.Parameters.Add("@CompanyFlag", SqlDbType.Bit, 0, "CompanyFlag")
                da.InsertCommand.Parameters.Add("@CompanyName", SqlDbType.VarChar, 255, "CompanyName")
                da.InsertCommand.Parameters.Add("@SplitMF", SqlDbType.Bit, 0, "SplitMF")
                da.InsertCommand.Parameters.Add("@AnniversaryDate", SqlDbType.SmallDateTime, 0, "AnniversaryDate")
                da.InsertCommand.Parameters.Add("@MaintenanceFeeAmount", SqlDbType.Money, 0, "MaintenanceFeeAmount")
                da.InsertCommand.Parameters.Add("@OriginallyWrittenDate", SqlDbType.SmallDateTime, 0, "OriginallyWrittenDate")
                da.InsertCommand.Parameters.Add("@MaintenanceFeeCodeID", SqlDbType.Int, 0, "MaintenanceFeeCodeID")
                da.InsertCommand.Parameters.Add("@MaintenanceFeeStatusID", SqlDbType.Int, 0, "MaintenanceFeeStatusID")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.Int, 0, "ContractID")
                parameter.Direction = ParameterDirection.Output

                dr = ds.Tables("Contract").NewRow
            End If
            Update_Field("ProspectID", _ProspectID, dr)
            Update_Field("TourID", _TourID, dr)
            Update_Field("LocationID", _LocationID, dr)
            Update_Field("ContractNumber", _ContractNumber, dr)
            If _ContractDate = "" Then _ContractDate = System.DBNull.Value.ToString
            If _ContractDate <> "" Then
                If Not (dr("ContractDate") Is System.DBNull.Value) Then
                    If _ContractDate <> CDate(CStr(dr("ContractDate"))).ToShortDateString Then
                        Update_Field("ContractDate", _ContractDate, dr)
                    End If
                Else
                    Update_Field("ContractDate", _ContractDate, dr)
                End If
            Else
                Update_Field("ContractDate", _ContractDate, dr)
            End If

            If _OccupancyDate = "" Then _OccupancyDate = System.DBNull.Value.ToString
            If _OccupancyDate <> "" Then
                If Not (dr("OccupancyDate") Is System.DBNull.Value) Then
                    If _OccupancyDate <> CDate(CStr(dr("OccupancyDate"))).ToShortDateString Then
                        Update_Field("OccupancyDate", _OccupancyDate, dr)
                    End If
                Else
                    Update_Field("OccupancyDate", _OccupancyDate, dr)
                End If
            Else
                Update_Field("OccupancyDate", _OccupancyDate, dr)
            End If
            'Update_Field("OccupancyDate", _OccupancyDate, dr)
            Update_Field("StatusID", _StatusID, dr)
            Update_Field("SubStatusID", _SubStatusID, dr)
            Update_Field("DeededStatusID", _DeededStatusID, dr)
            If _DeededDate = "" Then _DeededDate = System.DBNull.Value.ToString
            If _DeededDate <> "" Then
                If Not (dr("DeededDate") Is System.DBNull.Value) Then
                    If _DeededDate <> CDate(CStr(dr("DeededDate"))).ToShortDateString Then
                        Update_Field("DeededDate", _DeededDate, dr)
                    End If
                Else
                    Update_Field("DeededDate", _DeededDate, dr)
                End If
            Else
                Update_Field("DeededDate", _DeededDate, dr)
            End If
            'Update_Field("DeededDate", DeededDate, dr)
            
            Update_Field("FrequencyID", _FrequencyID, dr)
            Update_Field("WeekTypeID", _WeekTypeID, dr)
            Update_Field("SeasonID", _SeasonID, dr)
            Update_Field("CampaignID", _CampaignID, dr)
            Update_Field("PropertyTaxAmount", _PropertyTaxAmount, dr)
            Update_Field("BillingCodeID", _BillingCodeID, dr)
            Update_Field("SaleTypeID", _SaleTypeID, dr)
            Update_Field("SaleSubTypeID", _SaleSubType, dr)
            Update_Field("FundingStatusID", _FundingStatusID, dr)
            If _FundingDate = "" Then _FundingDate = System.DBNull.Value.ToString
            If _FundingDate <> "" Then
                If Not (dr("FundingDate") Is System.DBNull.Value) Then
                    If _OccupancyDate <> CDate(CStr(dr("FundingDate"))).ToShortDateString Then
                        Update_Field("FundingDate", _FundingDate, dr)
                    End If
                Else
                    Update_Field("FundingDate", _FundingDate, dr)
                End If
            Else
                Update_Field("FundingDate", _FundingDate, dr)
            End If
            'Update_Field("FundingDate", _FundingDate, dr)
            Update_Field("TypeID", _TypeID, dr)
            Update_Field("SubTypeID", _SubTypeID, dr)
            Update_Field("TrustName", _TrustName, dr)
            If _StatusDate <> "" Then
                If Not (dr("StatusDate") Is System.DBNull.Value) Then
                    If _StatusDate <> CDate(CStr(dr("StatusDate"))).ToShortDateString Then
                        Update_Field("StatusDate", _StatusDate, dr)
                    End If
                Else
                    Update_Field("StatusDate", _StatusDate, dr)
                End If
            Else
                Update_Field("StatusDate", _StatusDate, dr)
            End If
            'Update_Field("StatusDate", _StatusDate, dr)
            Update_Field("TrustFlag", _TrustFlag, dr)
            Update_Field("CompanyFlag", _CompanyFlag, dr)
            Update_Field("CompanyName", _CompanyName, dr)
            Update_Field("SplitMF", _SplitMF, dr)
            If _AnniversaryDate = "" Then _AnniversaryDate = System.DBNull.Value.ToString 'Then _AnniversaryDate = DBNull.Value & ""
            'Update_Field("AnniversaryDate", _AnniversaryDate, dr)
            If _AnniversaryDate <> "" Then
                If Not (dr("AnniversaryDate") Is System.DBNull.Value) Then
                    If _AnniversaryDate <> CDate(CStr(dr("AnniversaryDate"))).ToShortDateString Then
                        Update_Field("AnniversaryDate", _AnniversaryDate, dr)
                    End If
                Else
                    Update_Field("AnniversaryDate", _AnniversaryDate, dr)
                End If
            Else
                Update_Field("AnniversaryDate", _AnniversaryDate, dr)
            End If
            Update_Field("MaintenanceFeeAmount", _MaintenanceFeeAmount, dr)
            If _OriginallyWrittenDate <> "" Then
                If Not (dr("OriginallyWrittenDate") Is System.DBNull.Value) Then
                    If _OriginallyWrittenDate <> CDate(CStr(dr("OriginallyWrittenDate"))).ToShortDateString Then
                        Update_Field("OriginallyWrittenDate", _OriginallyWrittenDate, dr)
                    End If
                Else
                    Update_Field("OriginallyWrittenDate", _OriginallyWrittenDate, dr)
                End If
            Else
                Update_Field("OriginallyWrittenDate", _OriginallyWrittenDate, dr)
            End If
            'Update_Field("OriginallyWrittenDate", _OriginallyWrittenDate, dr)
            Update_Field("MaintenanceFeeStatusID", _MaintenanceFeeStatusID, dr)
            If _MaintenanceFeeCodeID > -1 Then
                Update_Field("MaintenanceFeeCodeID", _MaintenanceFeeCodeID, dr)

            End If
            If ds.Tables("Contract").Rows.Count < 1 Then ds.Tables("Contract").Rows.Add(dr)
            da.Update(ds, "Contract")
            _ID = ds.Tables("Contract").Rows(0).Item("ContractID")
            Return True

        Catch ex As Exception
            _Err = ex.ToString
            Return False
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
    End Function

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        'Dim ov As String = ""
        'If Right(sField, 4) = "Date" And drow(sField) = SqlDateTime.Null Then
        '    ov = ""
        'ElseIf drow(sField) Is System.DBNull.Value Then
        '    ov = ""
        'Else
        '    ov = CStr(drow(sField))
        'End If

        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField).ToString(), "") <> CStr(sValue) Then

            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "ContractID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            If Right(Trim(sField), 4) = "Date" And sValue = "" Then
                drow(sField) = DBNull.Value
            Else
                drow(sField) = sValue
            End If
            _Err = oEvents.Error_Message
        End If
    End Sub
    Public Function Verify_Contract(ByVal conNum As String) As Boolean
        Dim valid As Boolean = True
        Try
            cm.CommandText = "Select * from t_Contract where contractnumber = '" & conNum & "'"
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader()
            dread.Read()
            valid = dread.HasRows
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return valid
    End Function
    Public Function List_Inventory(ByVal conID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = "Select soi.SoldInventoryID, u.Name from t_SoldInventory soi inner join t_SalesInventory si on soi.salesinventoryid = si.salesinventoryid inner join t_Unit u on si.UnitID = u.UnitID where soi.contractid = '" & conID & "'"
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function
    Public Function Get_Contract_ID(ByVal conNum As String) As Integer
        Dim conID As Integer
        Try
            cm.CommandText = "Select contractID from t_Contract where ContractNumber = '" & conNum & "'"
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader()
            dread.Read()
            conID = dread.GetValue(0).ToString & ""
            dread = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return conID
    End Function
    Protected Overrides Sub Finalize()
        'If cn.State <> Data.ConnectionState.Closed Then cn.Close()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Function List_InventoryType(ByVal conNum As String) As String
        Dim invType As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(ut.ComboItem) as UnitType from t_Contract c inner join t_SoldInventory sdi on c.ContractID = sdi.ContractID inner join t_SalesInventory sli on sdi.SalesInventoryID = sli.SalesInventoryID inner join t_Unit u on sli.UnitID = u.UnitID inner join t_ComboItems ut on u.TypeID = ut.ComboItemID where c.ContractNumber = '" & conNum & "'"
            dread = cm.ExecuteReader
            Do While dread.Read
                If invType = "" Then
                    invType = dread("UnitType")
                Else
                    invType = invType & "," & dread("UnitType")
                End If
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return invType
    End Function
    Public Function List_CoOwners(ByVal conNum As String) As String
        Dim coOwners As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Case when cco.ProspectID = c.ProspectID then p.SpouseFirstname else p.Firstname end as FirstName, Case when cco.ProspectID = c.ProspectID then p.SpouseLastName else p.LastName end as LastName from t_ContractCoOwner cco inner join t_Contract c on cco.ContractID = c.ContractID inner join t_Prospect p on cco.ProspectID = p.ProspectID where c.ContractNumber = '" & conNum & "'"
            dread = cm.ExecuteReader
            Do While dread.Read
                If coOwners = "" Then
                    coOwners = dread("FirstName") & " " & dread("LastName")
                Else
                    coOwners = coOwners & ", " & dread("FirstName") & " " & dread("LastName")
                End If
            Loop
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return coOwners
    End Function
    Public Function Get_UnitWeek(ByVal conNum As String) As String
        Dim unitWeek As String = ""
        Dim lastDate As String = ""
        Dim f As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select c.OccupancyDate, f.Interval, h.Active, case when h.DateRemoved is null then '' else h.DateRemoved end as DateRemoved, u.Name as UnitName, i.Week from t_Contract c inner join t_Salesinventory2contracthist h on h.contractid = c.contractid inner join t_Frequency f on f.frequencyid = h.frequencyid inner join t_Salesinventory i on i.salesinventoryid = h.salesinventoryid inner join t_Unit u on u.unitid = i.unitid where c.ContractNumber = '" & conNum & "' order by active desc, salesinventorycontracthistid desc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read()
                    If dread("Interval") = "1" Then
                        f = "A"
                    ElseIf dread("Interval") = "2" Then
                        If Year(dread("OccupancyDate")) Mod 2 = 0 Then
                            f = "E"
                        Else
                            f = "0"
                        End If
                    ElseIf dread("Interval") = "3" Then
                        f = "T"
                    End If
                    If Not (dread("Active")) Then
                        If lastDate <> dread("DateRemoved").ToString Then
                            If unitWeek = "" Then
                                lastDate = dread("DateRemoved")
                                unitWeek = unitWeek & dread("UnitName") & "-" & dread("Week") & "-" & f
                            Else
                                Exit Do
                            End If
                        Else
                            If unitWeek <> "" Then
                                unitWeek = unitWeek & " / "
                            End If
                            unitWeek = unitWeek & dread("Unitname") & "-" & dread("Week") & "-" & f
                        End If
                    Else
                        If unitWeek <> "" Then
                            unitWeek = unitWeek & " / "
                        End If
                        unitWeek = unitWeek & dread("Unitname") & "-" & dread("Week") & "-" & f
                    End If
                Loop
            Else
                unitWeek = "None"
            End If
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return unitWeek
    End Function
    Public Function List_Pros_Contracts(ByVal prosID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select ContractID, Contractnumber from t_contract where prospectid = " & prosID & ""
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function Get_DP_Payment_Types(ByVal conNumber As String) As String
        Dim pTypes As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(pm.ComboItem) as Payment from t_payments p inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentiD inner join t_invoices i on pi.InvoiceID = i.InvoiceID inner join t_Fintranscodes f on i.FinTransID = f.FintransID inner join t_combOItems tc on f.TransCodeID = tc.ComboItemID inner join t_Mortgage m on i.KeyValue = m.MortgageID inner join t_contract c on m.ContractID = c.ContractID inner join t_ComboItems pm on p.MethodID = pm.ComboItemID where tc.ComboItem like '%Down Payment%' and i.KeyField = 'MortgageDP' and p.PosNeg = 1 and c.ContractNumber = '" & conNumber & "' and p.PaymentID not in (Select paymentid from v_Declined_CreditCards)"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If pTypes = "" Then
                        pTypes = dread("Payment")
                    Else
                        pTypes = pTypes & ", " & dread("Payment")
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return pTypes
    End Function

    Public Function Get_CC_Payment_Types(ByVal conNumber As String) As String
        Dim pTypes As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select Distinct(pm.ComboItem) as Payment from t_payments p inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentiD inner join t_invoices i on pi.InvoiceID = i.InvoiceID inner join t_Fintranscodes f on i.FinTransID = f.FintransID inner join t_combOItems tc on f.TransCodeID = tc.ComboItemID inner join t_Mortgage m on i.KeyValue = m.MortgageID inner join t_contract c on m.ContractID = c.ContractID inner join t_ComboItems pm on p.MethodID = pm.ComboItemID where tc.ComboItem like '%Closing Cost%' and i.KeyField = 'MortgageDP' and p.PosNeg = 1 and c.ContractNumber = '" & conNumber & "' and p.PaymentID not in (Select paymentid from v_Declined_CreditCards)"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If pTypes = "" Then
                        pTypes = dread("Payment")
                    Else
                        pTypes = pTypes & ", " & dread("Payment")
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return pTypes
    End Function

    Public Function Get_Next_Number() As Integer
        Dim conNum As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ContractNumber from t_ContractNumber"
            dread = cm.ExecuteReader
            dread.Read()
            conNum = dread("ContractNumber")
            dread.Close()
            cm.CommandText = "Update t_ContractNumber set contractnumber = contractnumber + 1"
            cm.ExecuteNonQuery()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return conNum
    End Function

    Public Function Get_Owner_Display(ByVal conID As Integer) As String
        Dim display As String = ""
        Try
            Dim oCon As New clsContract
            oCon.ContractID = conID
            oCon.Load()
            Dim multi As Boolean = False
            If oCon.CompanyName <> "" Then
                display = "<u>" & oCon.CompanyName.ToUpper & "</u>"
            Else
                Dim i As Integer = 0
                Dim sName() As String
                Dim oPros As New clsProspect
                oPros.Prospect_ID = oCon.ProspectID
                oPros.Load()
                If oPros.MiddleInit & "" = "" Then
                    display = oPros.First_Name.ToUpper & " <u>" & oPros.Last_Name.ToUpper & "</u>"
                Else
                    display = oPros.First_Name.ToUpper & " " & oPros.MiddleInit.ToUpper & ". <u>" & oPros.Last_Name.ToUpper & "</u>"
                End If
                If cn.State <> ConnectionState.Open Then cn.Open()
                cm.CommandText = "Select * from t_ContractCoOwner where contractID = " & conID & " and prospectID = " & oCon.ProspectID
                dread = cm.ExecuteReader
                If dread.HasRows Then
                    multi = True
                    dread.Read()
                    If oPros.SpouseLastName & "" = "" Then
                        If oPros.SpouseFirstName & "" <> "" Then
                            display = display & " and"
                            sName = oPros.SpouseFirstName.Split(" ")
                            If sName.Length > 1 Then
                                For i = 0 To sName.Length - 1
                                    display = display & " " & sName(i).ToUpper
                                Next
                                display = display & " " & "<u>" & sName(i).ToUpper & "</u>"
                            Else
                                display = display & " " & oPros.SpouseFirstName & " <u>" & oPros.Last_Name.ToUpper & "</u>"
                            End If
                        End If
                    Else
                        If oPros.SpouseFirstName & "" = "" Then
                            display = display & " and"
                            sName = oPros.SpouseLastName.Split(" ")
                            If sName.Length > 1 Then
                                For i = 0 To sName.Length - 1
                                    display = display & " " & sName(i).ToUpper
                                Next
                                display = display & " " & "<u>" & sName(i).ToUpper & "</u>"
                            Else
                                display = display & " " & oPros.SpouseLastName & " <u>" & oPros.Last_Name.ToUpper & "</u>"
                            End If
                        Else
                            display = display & " and " & oPros.SpouseFirstName.ToUpper & " <u>" & oPros.SpouseLastName.ToUpper & "</u>"
                        End If
                    End If
                End If
                dread.Close()
                cm.CommandText = "Select p.Firstname, p.LastName, p.MiddleInit, p.SpouseFirstname, p.SpouseLastname from t_ContractCoOwner co inner join t_Prospect p on co.ProspectID = p.ProspectID where co.ContractID = " & conID & " and co.ProspectID <> " & oCon.ProspectID
                dread = cm.ExecuteReader
                If dread.HasRows Then
                    Do While dread.Read()
                        display = display & ", and"
                        If dread("MiddleInit") & "" = "" Then
                            display = display & " " & dread("FirstName").ToUpper & " <u>" & dread("LastName").ToUpper & "</u>"
                        Else
                            display = display & " " & dread("FirstName").ToUpper & " " & dread("MiddleInit").ToUpper & ". <u>" & dread("LastName").ToUpper & "</u>"
                        End If

                        If dread("SpouseLastName") & "" = "" Then
                            If dread("SpouseFirstName") & "" <> "" Then
                                display = display & " And "
                                sName = dread("SpouseFirstName").Split(" ")
                                If sName.Length > 1 Then
                                    For i = 0 To sName.Length - 1
                                        display = display & " " & sName(i).ToUpper
                                    Next
                                    display = display & " " & "<u>" & sName(i).ToUpper & "</u>"
                                Else
                                    display = display & " " & dread("SpouseFirstName") & " <u> " & dread("LastName").ToUpper & "</u>"
                                End If
                            End If
                        Else
                            If dread("SpouseFirstName") & "" = "" Then
                                display = display & " And "
                                sName = dread("SpouseLastName").Split(" ")

                                If sName.Length > 1 Then
                                    For i = 0 To sName.Length - 1
                                        display = display & " " & sName(i).ToUpper
                                    Next
                                    display = display & " " & "<u>" & sName(i).ToUpper & "</u>"
                                Else
                                    display = display & " " & dread("SpouseLastName") & " <u> " & dread("LastName").ToUpper & "</u>"
                                End If
                            Else
                                display = display & " and " & dread("SpouseFirstName").ToUpper & " <u> " & dread("SpouseLastName").ToUpper & "</u>"
                            End If
                        End If
                    Loop
                End If
                dread.Close()
                If oCon.TrustName & "" <> "" Then
                    display = display & ", " & IIf(multi, "Trustees", "Trustee") & " of " & "<u>" & oCon.TrustName.ToUpper & "</u>"
                End If
                oPros = Nothing
            End If
            oCon = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return display
    End Function

    Public Function Get_Owner(ByVal conID As Integer, ByVal letter As Boolean) As String
        Dim display As String = ""
        Try
            Dim oCon As New clsContract
            oCon.ContractID = conID
            oCon.Load()
            Dim multi As Boolean = False
            Dim i As Integer = 0
            Dim sName() As String
            Dim oPros As New clsProspect
            oPros.Prospect_ID = oCon.ProspectID
            oPros.Load()
            If oPros.MiddleInit & "" = "" Then
                display = oPros.First_Name.ToUpper & " " & oPros.Last_Name.ToUpper & ""
            Else
                display = oPros.First_Name.ToUpper & " " & oPros.MiddleInit.ToUpper & ". " & oPros.Last_Name.ToUpper & ""
            End If
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ContractCoOwner where contractID = " & conID & " and prospectID = " & oCon.ProspectID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                multi = True
                dread.Read()
                If oPros.SpouseLastName & "" = "" Then
                    If oPros.SpouseFirstName & "" <> "" Then
                        display = display & " and"
                        sName = oPros.SpouseFirstName.Split(" ")
                        If sName.Length > 1 Then
                            For i = 0 To sName.Length - 1
                                display = display & " " & sName(i).ToUpper
                            Next
                            display = display & " " & "" & sName(i).ToUpper & ""
                        Else
                            display = display & " " & oPros.SpouseFirstName & " " & oPros.Last_Name.ToUpper & ""
                        End If
                    End If
                Else
                    If oPros.SpouseFirstName & "" = "" Then
                        display = display & " and"
                        sName = oPros.SpouseLastName.Split(" ")
                        If sName.Length > 1 Then
                            For i = 0 To sName.Length - 1
                                display = display & " " & sName(i).ToUpper
                            Next
                            display = display & " " & "" & sName(i).ToUpper & ""
                        Else
                            display = display & " " & oPros.SpouseLastName & " " & oPros.Last_Name.ToUpper & ""
                        End If
                    Else
                        display = display & " and " & oPros.SpouseFirstName.ToUpper & " " & oPros.SpouseLastName.ToUpper & ""
                    End If
                End If
            End If
            dread.Close()
            cm.CommandText = "Select p.Firstname, p.LastName, p.MiddleInit, p.SpouseFirstname, p.SpouseLastname from t_ContractCoOwner co inner join t_Prospect p on co.ProspectID = p.ProspectID where co.ContractID = " & conID & " and co.ProspectID <> " & oCon.ProspectID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read()
                    display = display & ", and"
                    If dread("MiddleInit") & "" = "" Then
                        display = display & " " & dread("FirstName").ToUpper & " " & dread("LastName").ToUpper & ""
                    Else
                        display = display & " " & dread("FirstName").ToUpper & " " & dread("MiddleInit").ToUpper & ". " & dread("LastName").ToUpper & ""
                    End If

                    If dread("SpouseLastName") & "" = "" Then
                        If dread("SpouseFirstName") & "" <> "" Then
                            display = display & " And "
                            sName = dread("SpouseFirstName").Split(" ")
                            If sName.Length > 1 Then
                                For i = 0 To sName.Length - 1
                                    display = display & " " & sName(i).ToUpper
                                Next
                                display = display & " " & "" & sName(i).ToUpper & ""
                            Else
                                display = display & " " & dread("SpouseFirstName") & "  " & dread("LastName").ToUpper & ""
                            End If
                        End If
                    Else
                        If dread("SpouseFirstName") & "" = "" Then
                            display = display & " And "
                            sName = dread("SpouseLastName").Split(" ")

                            If sName.Length > 1 Then
                                For i = 0 To sName.Length - 1
                                    display = display & " " & sName(i).ToUpper
                                Next
                                display = display & " " & "" & sName(i).ToUpper & ""
                            Else
                                display = display & " " & dread("SpouseLastName") & "  " & dread("LastName").ToUpper & ""
                            End If
                        Else
                            display = display & " and " & dread("SpouseFirstName").ToUpper & "  " & dread("SpouseLastName").ToUpper & ""
                        End If
                    End If
                Loop
            End If
            dread.Close()
            If letter = False Then
                If oCon.TrustName & "" <> "" Then
                    display = display & ", " & IIf(multi, "Trustees", "Trustee") & " of " & "" & oCon.TrustName.ToUpper & ""
                End If
                If oCon.CompanyName <> "" Then
                    display = display & ", of " & oCon.CompanyName
                End If
            End If
            oPros = Nothing

            oCon = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return display
    End Function

    Public Function Get_Unit_Display(ByVal conID As Integer) As String
        Dim display As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ut.ComboItem as UnitType, u.Name from t_SoldInventory sdi inner join t_SalesInventory si on sdi.SalesInventoryID = si.SalesInventoryID inner join t_Unit u on si.UnitID = u.UnitID inner join t_comboitems ut on u.TypeID = ut.ComboitemID where sdi.ContractID = " & conID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If display = "" Then
                        display = dread("Name")
                    Else
                        display = display & " and " & dread("Name")
                    End If
                    If dread("UnitType") = "Cottage" Then
                        display = display & " A & B"
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return display
    End Function

    Public Function Get_Old_Unit_Display(ByVal conID As Integer) As String
        Dim display As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ut.ComboItem as UnitType, u.Name from t_SalesInventory2ContractHist sdi inner join t_SalesInventory si on sdi.SalesInventoryID = si.SalesInventoryID inner join t_Unit u on si.UnitID = u.UnitID inner join t_comboitems ut on u.TypeID = ut.ComboitemID where sdi.ContractID = '" & conID & "' and sdi.HideFromContract = 0 and sdi.DateRemoved is null"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If display = "" Then
                        display = dread("Name")
                    Else
                        display = display & " and " & dread("Name")
                    End If
                    If dread("UnitType") = "Cottage" Then
                        display = display & " A & B"
                    End If
                Loop
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return display
    End Function

    Public Function Get_Week_Display(ByVal conID As Integer) As String
        Dim display As String = ""
        Dim multi As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select si.Week, sdi.OccupancyYear, f.Frequency from t_SoldInventory sdi inner join t_SalesInventory si on sdi.SalesInventoryID = si.SalesInventoryID inner join t_Frequency f on sdi.FrequencyID = f.FrequencyID where sdi.ContractID = " & conID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If display = "" Then
                        display = dread("Week") & " - " & dread("Frequency")
                    Else
                        display = display & ", " & dread("Week") & " - " & dread("Frequency")
                        multi = True
                    End If
                    If dread("Frequency") = "Biennial" Then
                        If dread("OccupancyYear") Mod 2 = 0 Then
                            display = display & " - Even"
                        Else
                            display = display & " - Odd"
                        End If
                    End If
                Loop
                If multi Then
                    display = display & " Respectively"
                End If
            End If
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return display
    End Function

    Public Function Get_Old_Week_Display(ByVal conID As Integer) As String
        Dim display As String = ""
        Dim multi As Boolean = False
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select si.Week, sdi.OccupancyYear, f.Frequency from t_SalesInventory2ContractHist sdi inner join t_SalesInventory si on sdi.SalesInventoryID = si.SalesInventoryID inner join t_Frequency f on sdi.FrequencyID = f.FrequencyID where sdi.ContractID = '" & conID & "' and sdi.HideFromContract = 0 and sdi.DateRemoved is null"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read
                    If display = "" Then
                        display = dread("Week") & " - " & dread("Frequency")
                    Else
                        display = display & ", " & dread("Week") & " - " & dread("Frequency")
                        multi = True
                    End If
                    If dread("Frequency") = "Biennial" Then
                        If dread("OccupancyYear") Mod 2 = 0 Then
                            display = display & " - Even"
                        Else
                            display = display & " - Odd"
                        End If
                    End If
                Loop
                If multi Then
                    display = display & " Respectively"
                End If
            End If
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return display
    End Function

    Public Function Get_CheckIn_Day(ByVal conID As Integer) As String
        Dim weekType As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select top 1 e.Comboitem as CheckInDay from t_SoldInventory sdi inner join t_SalesInventory si on sdi.SalesInventoryID = si.SalesInventoryID inner join t_Unit u on si.UnitID = u.UnitID inner join t_Room r on r.UnitID = u.UnitID inner join t_ComboItems e on r.SubTypeID = e.ComboItemID where sdi.ContractID = " & conID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                Select Case dread("CheckInDay")
                    Case "FRI"
                        weekType = " Friday "
                    Case "SAT"
                        weekType = " Saturday "
                    Case "SUN"
                        weekType = " Sunday "
                    Case "THU"
                        weekType = " Thursday "
                    Case "MON"
                        weekType = " Monday "
                    Case "TUE"
                        weekType = " Tuesday "
                    Case "WED"
                        weekType = " Wednesday "
                End Select
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return weekType
    End Function

    Public Function Get_Unit_Size(ByVal conID As Integer) As String
        Dim size As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from v_ContractInventory where contractID = '" & conID & "'"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                size = (dread("CottageInventory") * 10) + (dread("TownesInventory") * 6) + (dread("EstatesInventory2BD") * 6) + (dread("EstatesInventory1BDDWN") * 4) + (dread("EstatesInventory1BDUP") * 4)
                'If dread("BD") = "2BD" Then
                '    size = "6"
                'ElseIf dread("BD") = "3BD" Then
                '    size = "10"
                'ElseIf dread("BD") = "1BD" Then
                '    size = "4"
                'ElseIf dread("BD") = "4BD" And dread("SaleType") = "Estates" Then
                '    size = "14"
                'ElseIf dread("BD") = "4BD" And dread("SaleType") = "Townes" Then
                '    size = "12"
                'ElseIf dread("BD") = "Unknown" Then
                '    If CInt(dread("EstatesInventory2BD")) + CInt(dread("EstatesInventory1BDDWN")) + CInt(dread("EstatesInventory1BDUP")) = 4 Then
                '        size = "14"
                '    ElseIf CInt(dread("EstatesInventory2BD")) + CInt(dread("EstatesInventory1BDDWN")) + CInt(dread("EstatesInventory1BDUP")) = 3 Then
                '        size = "10"
                '    ElseIf CInt(dread("EstatesInventory2BD")) + CInt(dread("EstatesInventory1BDDWN")) + CInt(dread("EstatesInventory1BDUP")) = 2 Then
                '        size = "6"
                '    Else
                '        size = "____"
                '    End If
                'Else
                '    size = "____"
                'End If
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return size
    End Function

    Public Function Get_Inventory(ByVal conID As Integer) As String
        Dim inv As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select ContractID, BD, Frequency, SaleType, SUM(EstatesInventory1BDDWN + EstatesInventory1BDUP + EstatesInventory2BD) as TotalBD   from v_ContractInventory where ContractID = '" & conID & "' Group by ContractID, BD, Frequency, SaleType"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If dread("BD") = "Unknown" Then
                    inv = dread("TotalBD") & " - Bedroom " & dread("Frequency") & " " & dread("SaleType")
                Else
                    inv = Left(dread("BD"), 1) & " - Bedroom " & dread("Frequency") & " " & dread("SaleType")
                End If
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return inv
    End Function

    Public Function Get_Signatures(ByVal conID As Integer, ByVal old As Boolean) As DataTable
        Dim dt As New DataTable
        If old Then
            dt.TableName = "OldSignatures"
        Else
            dt.TableName = "Signatures"
        End If
        dt.Columns.Add("Signature")
        dt.Columns.Add("Signature2")
        Dim display As String
        Try
            Dim colCount As Integer = 0
            Dim oCon As New clsContract
            oCon.ContractID = conID
            oCon.Load()
            Dim multi As Boolean = False
            Dim i As Integer = 0
            Dim sName() As String
            Dim oPros As New clsProspect
            Dim dRow As DataRow
            oPros.Prospect_ID = oCon.ProspectID
            oPros.Load()
            If oPros.MiddleInit & "" = "" Then
                display = oPros.First_Name.ToUpper & " " & oPros.Last_Name.ToUpper & ""
            Else
                display = oPros.First_Name.ToUpper & " " & oPros.MiddleInit.ToUpper & ". " & oPros.Last_Name.ToUpper & ""
            End If
            If oCon.TrustName <> "" Then
                display = display & ", Trustee<br>" & oCon.TrustName
            ElseIf oCon.CompanyName <> "" Then
                display = display & "<br>" & oCon.CompanyName
            End If
            dRow = dt.NewRow
            dRow("Signature") = display
            display = ""
            colCount = 1
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ContractCoOwner where contractID = " & conID & " and prospectID = " & oCon.ProspectID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                multi = True
                dread.Read()
                If oPros.SpouseLastName & "" = "" Then
                    If oPros.SpouseFirstName & "" <> "" Then
                        sName = oPros.SpouseFirstName.Split(" ")
                        If sName.Length > 1 Then
                            For i = 0 To sName.Length - 1
                                display = display & " " & sName(i).ToUpper
                            Next
                            display = display & " " & "" & sName(i).ToUpper & ""
                        Else
                            display = display & " " & oPros.SpouseFirstName & " " & oPros.Last_Name.ToUpper & ""
                        End If
                    End If
                Else
                    If oPros.SpouseFirstName & "" = "" Then
                        sName = oPros.SpouseLastName.Split(" ")
                        If sName.Length > 1 Then
                            For i = 0 To sName.Length - 1
                                display = display & " " & sName(i).ToUpper
                            Next
                            display = display & " " & "" & sName(i).ToUpper & ""
                        Else
                            display = display & " " & oPros.SpouseLastName & " " & oPros.Last_Name.ToUpper & ""
                        End If
                    Else
                        display = display & oPros.SpouseFirstName.ToUpper & " " & oPros.SpouseLastName.ToUpper & ""
                    End If
                End If
                If oCon.TrustName <> "" Then
                    display = display & ", Trustee<br>" & oCon.TrustName
                ElseIf oCon.CompanyName <> "" Then
                    display = display & "<br>" & oCon.CompanyName
                End If
                dRow("Signature2") = display
                dt.Rows.Add(dRow)
                colCount = 0
            End If
            dread.Close()
            cm.CommandText = "Select p.Firstname, p.LastName, p.MiddleInit, p.SpouseFirstname, p.SpouseLastname from t_ContractCoOwner co inner join t_Prospect p on co.ProspectID = p.ProspectID where co.ContractID = " & conID & " and co.ProspectID <> " & oCon.ProspectID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read()
                    display = ""
                    If dread("MiddleInit") & "" = "" Then
                        display = display & " " & dread("FirstName").ToUpper & " " & dread("LastName").ToUpper & ""
                    Else
                        display = display & " " & dread("FirstName").ToUpper & " " & dread("MiddleInit").ToUpper & ". " & dread("LastName").ToUpper & ""
                    End If
                    If oCon.TrustName <> "" Then
                        display = display & ", Trustee<br>" & oCon.TrustName
                    ElseIf oCon.CompanyName <> "" Then
                        display = display & "<br>" & oCon.CompanyName
                    End If
                    If colCount = 1 Then
                        dRow("Signature2") = display
                        dt.Rows.Add(dRow)
                        colCount = 0
                    Else
                        dRow = dt.NewRow
                        dRow("Signature") = display
                        colCount = 1
                    End If
                    display = ""
                    If dread("SpouseLastName") & "" = "" Then
                        If dread("SpouseFirstName") & "" <> "" Then
                            sName = dread("SpouseFirstName").Split(" ")
                            If sName.Length > 1 Then
                                For i = 0 To sName.Length - 1
                                    If display = "" Then
                                        display = sName(i).ToUpper
                                    Else
                                        display = display & " " & sName(i).ToUpper
                                    End If
                                Next
                                display = display & " " & "" & sName(i).ToUpper & ""
                            Else
                                display = dread("SpouseFirstName") & "  " & dread("LastName").ToUpper & ""
                            End If
                            If oCon.TrustName <> "" Then
                                display = display & ", Trustee<br>" & oCon.TrustName
                            ElseIf oCon.CompanyName <> "" Then
                                display = display & "<br>" & oCon.CompanyName
                            End If
                            If colCount = 1 Then
                                dRow("Signature2") = display
                                dt.Rows.Add(dRow)
                                colCount = 0
                            Else
                                dRow = dt.NewRow
                                dRow("Signature") = display
                                colCount = 1
                            End If
                        End If
                    Else
                        If dread("SpouseFirstName") & "" = "" Then
                            sName = dread("SpouseLastName").Split(" ")
                            If sName.Length > 1 Then
                                For i = 0 To sName.Length - 1
                                    If display = "" Then
                                        display = sName(i).ToUpper
                                    Else
                                        display = display & " " & sName(i).ToUpper
                                    End If
                                Next
                                display = display & " " & "" & sName(i).ToUpper & ""
                            Else
                                display = dread("SpouseLastName") & "  " & dread("LastName").ToUpper & ""
                            End If
                        Else
                            display = display & dread("SpouseFirstName").ToUpper & "  " & dread("SpouseLastName").ToUpper & ""
                        End If
                        If oCon.TrustName <> "" Then
                            display = display & ", Trustee<br>" & oCon.TrustName
                        ElseIf oCon.CompanyName <> "" Then
                            display = display & "<br>" & oCon.CompanyName
                        End If
                        If colCount = 1 Then
                            dRow("Signature2") = display
                            dt.Rows.Add(dRow)
                            colCount = 0
                        Else
                            dRow = dt.NewRow
                            dRow("Signature") = display
                            colCount = 1
                        End If
                    End If
                Loop
                If colCount = 1 Then
                    dt.Rows.Add(dRow)
                End If
            Else
                If colCount = 1 Then
                    dt.Rows.Add(dRow)
                End If
            End If
            dread.Close()
            oPros = Nothing
            oCon = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dt
    End Function

    Public Function Get_Owners(ByVal conID As Integer) As DataTable
        Dim dt As New DataTable
        dt.TableName = "Owners"
        dt.Columns.Add("Owner")
        dt.Columns.Add("Address1")
        dt.Columns.Add("Address2")
        dt.Columns.Add("SSN")
        dt.Columns.Add("Company")
        dt.Columns.Add("ContractNumber")
        Try
            Dim oCon As New clsContract
            oCon.ContractID = conID
            oCon.Load()
            Dim multi As Boolean = False
            Dim i As Integer = 0
            Dim sName() As String
            Dim oPros As New clsProspect
            Dim oAdd As New clsAddress
            Dim oCombo As New clsComboItems
            Dim dRow As DataRow
            Dim display As String = ""
            oPros.Prospect_ID = oCon.ProspectID
            oPros.Load()
            If oPros.MiddleInit & "" = "" Then
                display = oPros.First_Name.ToUpper & " " & oPros.Last_Name.ToUpper & ""
            Else
                display = oPros.First_Name.ToUpper & " " & oPros.MiddleInit.ToUpper & ". " & oPros.Last_Name.ToUpper & ""
            End If
            oAdd.AddressID = oAdd.Get_Contract_Address(oPros.Prospect_ID)
            oAdd.Load()
            dRow = dt.NewRow
            dRow("Owner") = display
            dRow("Address1") = oAdd.Address1 & IIF(oAdd.Address2 & "" <> "", " " & oAdd.Address2, "")
            dRow("Address2") = oAdd.City & ", " & oCombo.Lookup_ComboItem(oAdd.StateID) & " " & oAdd.PostalCode
            If oPros.SSN <> "" Then
                dRow("SSN") = Left(oPros.SSN, 3) & "  -  " & Right(Left(oPros.SSN, 5), 2) & "  -  " & Right(oPros.SSN, 4)
            Else
                dRow("SSN") = "               -            -                "
            End If
            dRow("Company") = oCon.CompanyName
            dRow("ContractNumber") = oCon.ContractNumber
            dt.Rows.Add(dRow)
                display = ""
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_ContractCoOwner where contractID = " & conID & " and prospectID = " & oCon.ProspectID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                multi = True
                dread.Read()
                If oPros.SpouseLastName & "" = "" Then
                    If oPros.SpouseFirstName & "" <> "" Then
                        sName = oPros.SpouseFirstName.Split(" ")
                        If sName.Length > 1 Then
                            For i = 0 To sName.Length - 1
                                display = display & " " & sName(i).ToUpper
                            Next
                            display = display & " " & "" & sName(i).ToUpper & ""
                        Else
                            display = display & " " & oPros.SpouseFirstName & " " & oPros.Last_Name.ToUpper & ""
                        End If
                    End If
                Else
                    If oPros.SpouseFirstName & "" = "" Then
                        sName = oPros.SpouseLastName.Split(" ")
                        If sName.Length > 1 Then
                            For i = 0 To sName.Length - 1
                                display = display & " " & sName(i).ToUpper
                            Next
                            display = display & " " & "" & sName(i).ToUpper & ""
                        Else
                            display = display & " " & oPros.SpouseLastName & " " & oPros.Last_Name.ToUpper & ""
                        End If
                    Else
                        display = display & oPros.SpouseFirstName.ToUpper & " " & oPros.SpouseLastName.ToUpper & ""
                    End If
                End If
                dRow = dt.NewRow
                dRow("Owner") = display
                dRow("Address1") = oAdd.Address1 & IIF(oAdd.Address2 & "" <> "", " " & oAdd.Address2, "")
                dRow("Address2") = oAdd.City & ", " & oCombo.Lookup_ComboItem(oAdd.StateID) & " " & oAdd.PostalCode
                If oPros.SpouseSSN <> "" Then
                    dRow("SSN") = Left(oPros.SpouseSSN, 3) & "  -  " & Right(Left(oPros.SpouseSSN, 5), 2) & "  -  " & Right(oPros.SpouseSSN, 4)
                Else
                    dRow("SSN") = "               -            -                "
                End If
                dRow("Company") = oCon.CompanyName
                dRow("ContractNumber") = oCon.ContractNumber
                dt.Rows.Add(dRow)
            End If
            dread.Close()
            cm.CommandText = "Select p.ProspectID, p.Firstname, p.LastName, p.MiddleInit, p.SSN, p.SpouseFirstname, p.SpouseLastname, p.SpouseSSN from t_ContractCoOwner co inner join t_Prospect p on co.ProspectID = p.ProspectID where co.ContractID = " & conID & " and co.ProspectID <> " & oCon.ProspectID
            dread = cm.ExecuteReader
            If dread.HasRows Then
                Do While dread.Read()
                    display = ""
                    If dread("MiddleInit") & "" = "" Then
                        display = display & " " & dread("FirstName").ToUpper & " " & dread("LastName").ToUpper & ""
                    Else
                        display = display & " " & dread("FirstName").ToUpper & " " & dread("MiddleInit").ToUpper & ". " & dread("LastName").ToUpper & ""
                    End If
                    oAdd.AddressID = oAdd.Get_Contract_Address(dread("ProspectID"))
                    oAdd.Load()
                    dRow = dt.NewRow
                    dRow("Owner") = display
                    dRow("Address1") = IIf(oAdd.AddressID > 0, oAdd.Address1 & IIF(oAdd.Address2 & "" <> "", " " & oAdd.Address2, ""), "")
                    dRow("Address2") = IIf(oAdd.AddressID > 0, oAdd.City & ", " & oCombo.Lookup_ComboItem(oAdd.StateID) & " " & oAdd.PostalCode, "")
                    If dread("SSN") & "" <> "" Then
                        dRow("SSN") = Left(dread("SSN"), 3) & "  -  " & Right(Left(dread("SSN"), 5), 2) & "  -  " & Right(dread("SSN"), 4)
                    Else
                        dRow("SSN") = "               -            -                "
                    End If
                    dRow("Company") = oCon.CompanyName
                    dRow("ContractNumber") = oCon.ContractNumber
                    dt.Rows.Add(dRow)
                    display = ""
                    If dread("SpouseLastName") & "" = "" Then
                        If dread("SpouseFirstName") & "" <> "" Then
                            sName = dread("SpouseFirstName").Split(" ")
                            If sName.Length > 1 Then
                                For i = 0 To sName.Length - 1
                                    If display = "" Then
                                        display = sName(i).ToUpper
                                    Else
                                        display = display & " " & sName(i).ToUpper
                                    End If
                                Next
                                display = display & " " & "" & sName(i).ToUpper & ""
                            Else
                                display = dread("SpouseFirstName") & "  " & dread("LastName").ToUpper & ""
                            End If
                            dRow = dt.NewRow
                            dRow("Owner") = display
                            dRow("Address1") = IIf(oAdd.AddressID > 0, oAdd.Address1 & IIF(oAdd.Address2 & "" <> "", " " & oAdd.Address2, ""), "")
                            dRow("Address2") = IIf(oAdd.AddressID > 0, oAdd.City & ", " & oCombo.Lookup_ComboItem(oAdd.StateID) & " " & oAdd.PostalCode, "")
                            If dread("SpouseSSN") & "" <> "" Then
                                dRow("SSN") = Left(dread("SpouseSSN"), 3) & "  -  " & Right(Left(dread("SpouseSSN"), 5), 2) & "  -  " & Right(dread("SpouseSSN"), 4)
                            Else
                                dRow("SSN") = "               -            -                "
                            End If
                            dRow("Company") = oCon.CompanyName
                            dRow("ContractNumber") = oCon.ContractNumber
                            dt.Rows.Add(dRow)

                        End If
                    Else
                        If dread("SpouseFirstName") & "" = "" Then
                            sName = dread("SpouseLastName").Split(" ")
                            If sName.Length > 1 Then
                                For i = 0 To sName.Length - 1
                                    If display = "" Then
                                        display = sName(i).ToUpper
                                    Else
                                        display = display & " " & sName(i).ToUpper
                                    End If
                                Next
                                display = display & " " & "" & sName(i).ToUpper & ""
                            Else
                                display = dread("SpouseLastName") & "  " & dread("LastName").ToUpper & ""
                            End If
                        Else
                            display = display & dread("SpouseFirstName").ToUpper & "  " & dread("SpouseLastName").ToUpper & ""
                        End If
                        dRow = dt.NewRow
                        dRow("Owner") = display
                        dRow("Address1") = IIf(oAdd.AddressID > 0, oAdd.Address1 & IIF(oAdd.Address2 & "" <> "", " " & oAdd.Address2, ""), "")
                        dRow("Address2") = IIf(oAdd.AddressID > 0, oAdd.City & ", " & oCombo.Lookup_ComboItem(oAdd.StateID) & " " & oAdd.PostalCode, "")
                        If dread("SpouseSSN") & "" <> "" Then
                            dRow("SSN") = Left(dread("SpouseSSN"), 3) & "  -  " & Right(Left(dread("SpouseSSN"), 5), 2) & "  -  " & Right(dread("SpouseSSN"), 4)
                        Else
                            dRow("SSN") = "               -            -                "
                        End If
                        dRow("Company") = oCon.CompanyName
                        dRow("ContractNumber") = oCon.ContractNumber
                        dt.Rows.Add(dRow)
                    End If
                Loop
            End If
            dread.Close()
            oPros = Nothing
            oCon = Nothing
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dt
    End Function

    Public Function Get_Closing_Officer(ByVal conID As Integer) As String
        Dim co As String = ""
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select p.Firstname, p.LastName from t_Personnel p inner join t_personnelTrans pt on p.PersonnelID = pt.PersonnelID inner join t_ComboItems ptt on pt.TitleID = ptt.ComboItemID where pt.KeyField = 'ContractID' and pt.keyvalue = '" & conID & "' and ptt.ComboItem in ('Closer','Closing Officer') order by ptt.ComboItem"
            If dread.HasRows Then
                dread.Read()
                co = dread("FirstName") & " " & dread("LastName")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return co
    End Function

    Public Function Get_MF_Code(ByVal conID As Integer) As Integer
        Dim mfCode As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select mfc.MaintenanceFeeCodeID from t_MaintenanceFeeCodes mfc inner join " & _
                        "(Select case when SaleType = 'Cottage' then " & _
                            " 	case when contractdate < '11/18/05' then " & _
                             "            'KCP01' " & _
                            " 	else " & _
                            " 		case when cottageinventory = 1 and contractdate < '2/25/08' then " & _
                             "            'KCP04' " & _
                            " 		when cottageinventory = 1 and contractdate > '2/24/08' then " & _
                             "            'KCP11' " & _
                            " 		else " & _
                            "             'KCP20' " & _
                              "           End " & _
                             "            End " & _
                            " when saletype = 'Townes' then " & _
                            " 	case when contractdate < '11/18/05' then " & _
                            " 		case when townesinventory = 1 then " & _
                             "            'KCP02' " & _
                            " 		when townesinventory = 2 then " & _
                             "            'KCP03' " & _
                            " 		else  " & _
                              "           'KCP00' " & _
                             "            End " & _
                            " 	when contractdate < '2/25/08' then " & _
                            " 		case when townesinventory = 1 then " & _
                             "            'KCP05' " & _
                            " 		when townesinventory = 2 then " & _
                             "            'KCP06' " & _
                            " 		else  " & _
                              "           'KCP00' " & _
                             "            End " & _
                            " 	else " & _
                            " 		case when townesinventory = 1 then " & _
                             "            'KCP12' " & _
                            " 		when townesinventory = 2 then " & _
                            " 			case when contractdate < '12/14/09' then " & _
                             "            'KCP10' " & _
                            " 			else " & _
                              "           'KCP13' " & _
                             "            End " & _
                            " 		when townesinventory = 4 then " & _
                             "            'KCP22' " & _
                            " 		else " & _
                               "          'KCP00' " & _
                              "           End " & _
                             "            End " & _
                            " when saletype = 'Combo' then " & _
                            " 	case when contractdate < '7/1/13' then " & _
                             "            'KCP18' " & _
                            " 	else " & _
                            "             'KCP19' " & _
                             "            End " & _
                            " when saletype = 'Estates' then " & _
                            " 	case when contractdate between '11/18/05' and '2/25/08' then " & _
                            " 		case when estatesinventory2bd = 1 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 1 then " & _
                             "            'KCP09' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 0 then " & _
                             "            'KCP08' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 0 then " & _
                             "            'KCP00' " & _
                            " 		when estatesinventory2bd = 0 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 0 then " & _
                            "             'KCP07' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 1 then " & _
                             "            'KCP07' " & _
                            " 		when estatesinventory2bd = 0 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 1 then " & _
                             "            'KCP07' " & _
                            " 		else  " & _
                              "           'KCP00' " & _
                             "            End " & _
                            " 	else " & _
                            " 		case when estatesinventory2bd = 1 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 1 then " & _
                             "            'KCP17' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 0 then " & _
                             "            'KCP16' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 0 then " & _
                             "            'KCP15' " & _
                            " 		when estatesinventory2bd = 0 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 0 then " & _
                             "            'KCP14' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 1 then " & _
                             "            'KCP14' " & _
                            " 		when estatesinventory2bd = 0 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 1 then " & _
                             "            'KCP15' " & _
                            " 		when estatesinventory2bd = 0 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 1 then " & _
                             "            'KCP14' " & _
                            " 		else  " & _
                            " 			case when estatesinventory2bd = 2 and estatesinventory1bddwn = 2 and estatesinventory1bdup = 2 then " & _
                             "            'KCP23' " & _
                            " 			when estatesinventory2bd = 0 and estatesinventory1bddwn + estatesinventory1bdup = 8 then " & _
                             "            'KCP24' " & _
                            " 			when estatesinventory2bd = 3 and estatesinventory1bddwn = 3 and estatesinventory1bdup = 3 then " & _
                            "             'KCP25' " & _
                            " 			else " & _
                            " 				case when (cottageinventory * 3) + (townesinventory * 2) + (estatesinventory2bd * 2) + (estatesinventory1bddwn * 1) + (estatesinventory1bdup * 1) = 2 then " & _
                             "            'KCP26' " & _
                            " 				when (cottageinventory * 3) + (townesinventory * 2) + (estatesinventory2bd * 2) + (estatesinventory1bddwn * 1) + (estatesinventory1bdup * 1) = 3 then " & _
                             "            'KCP27' " & _
                            " 				when (cottageinventory * 3) + (townesinventory * 2) + (estatesinventory2bd * 2) + (estatesinventory1bddwn * 1) + (estatesinventory1bdup * 1) = 4 then " & _
                             "            'KCP28' " & _
                            " 				else " & _
                              "           'KCP00' " & _
                             "            End " & _
                            "             End " & _
                           "              End " & _
                          "               End " & _
                         "                End as MFCode from v_ContractInventory where contractid = '" & conID & "') c on mfc.Code = c.MFCode"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                mfCode = dread("MaintenanceFeeCodeID")
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return mfCode
    End Function

    Public Function get_CE_Dues_Amt(ByVal prosID As Integer) As Integer
        Dim dues As Integer = 0
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select top 1 ci.frequency from v_ContractInventory ci inner join t_Contract c on ci.ContractID = c.ContractID left outer join t_CombOitems st on c.SubTypeID = st.ComboItemID  where ci.ContractStatus in ('Active','On Hold','Pender') and ci.prospectid = '" & prosID & "' and st.ComboItem = 'Points' order by ci.frequency asc"
            dread = cm.ExecuteReader
            If dread.HasRows Then
                dread.Read()
                If dread("Frequency") = "Annual" Then
                    dues = 179
                ElseIf dread("Frequency") = "Biennial" Then
                    dues = 132
                ElseIf dread("Frequency") = "Triennial" Then
                    dues = 117
                End If
            End If
            dread.Close()
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
        End Try
        Return dues
    End Function

    Public Function Verify_Contract_ID(ByVal conNum As String) As Boolean
        Dim valid As Boolean = True
        Try
            cm.CommandText = "Select * from t_Contract where contractID = '" & conNum & "'"
            If cn.State <> ConnectionState.Open Then cn.Open()
            dread = cm.ExecuteReader()
            dread.Read()
            valid = dread.HasRows
        Catch ex As Exception
            _Err = ex.Message
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        Return valid
    End Function

    Public Property SubStatusID As Integer
        Get
            Return _SubStatusID
        End Get
        Set(ByVal value As Integer)
            _SubStatusID = value
        End Set
    End Property

    Public ReadOnly Property Error_Message() As String
        Get
            Return _Err
        End Get
    End Property

    Public Property MaintenanceFeeCodeID As Integer
        Get
            Return _MaintenanceFeeCodeID
        End Get
        Set(ByVal value As Integer)
            _MaintenanceFeeCodeID = value
        End Set
    End Property

    Public Property MaintenanceFeeAmount As Decimal
        Get
            Return _MaintenanceFeeAmount
        End Get
        Set(ByVal value As Decimal)
            _MaintenanceFeeAmount = value
        End Set
    End Property

    Public Property ContractID() As Integer
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

    Public Property ProspectID() As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
        End Set
    End Property

    Public Property TourID() As Integer
        Get
            Return _TourID
        End Get
        Set(ByVal value As Integer)
            _TourID = value
        End Set
    End Property

    Public Property LocationID() As Integer
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Integer)
            _LocationID = value
        End Set
    End Property

    Public Property ContractNumber() As String
        Get
            Return _ContractNumber
        End Get
        Set(ByVal value As String)
            _ContractNumber = value
        End Set
    End Property

    Public Property ContractDate() As String
        Get
            Return _ContractDate
        End Get
        Set(ByVal value As String)
            _ContractDate = value
        End Set
    End Property

    Public Property OccupancyDate() As String
        Get
            Return _OccupancyDate
        End Get
        Set(ByVal value As String)
            _OccupancyDate = value
        End Set
    End Property

    Public Property IgnoreStatusDate As Boolean
        Get
            Return _IgnoreStatusDate
        End Get
        Set(value As Boolean)
            _IgnoreStatusDate = value
        End Set
    End Property

    Public Property StatusID() As Integer
        Get
            Return _StatusID
        End Get
        Set(ByVal value As Integer)
            If value <> _StatusID And Not (_IgnoreStatusDate) Then
                _StatusDate = System.DateTime.Now.ToShortDateString
            End If
            _StatusID = value
        End Set
    End Property

    Public Property DeededStatusID() As Integer
        Get
            Return _DeededStatusID
        End Get
        Set(ByVal value As Integer)
            _DeededStatusID = value
        End Set
    End Property

    Public Property StatusDate() As String
        Get
            Return _StatusDate
        End Get
        Set(ByVal value As String)
            _StatusDate = value
        End Set
    End Property


    Public Property DeededDate() As String
        Get
            Return _DeededDate
        End Get
        Set(ByVal value As String)
            _DeededDate = value
        End Set
    End Property

    Public Property FrequencyID() As Integer
        Get
            Return _FrequencyID
        End Get
        Set(ByVal value As Integer)
            _FrequencyID = value
        End Set
    End Property

    Public Property WeekTypeID() As Integer
        Get
            Return _WeekTypeID
        End Get
        Set(ByVal value As Integer)
            _WeekTypeID = value
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

    Public Property CampaignID() As Integer
        Get
            Return _CampaignID
        End Get
        Set(ByVal value As Integer)
            _CampaignID = value
        End Set
    End Property

    Public Property PropertyTaxAmount() As Decimal
        Get
            Return _PropertyTaxAmount
        End Get
        Set(ByVal value As Decimal)
            _PropertyTaxAmount = value
        End Set
    End Property

    Public Property BillingCodeID() As Integer
        Get
            Return _BillingCodeID
        End Get
        Set(ByVal value As Integer)
            _BillingCodeID = value
        End Set
    End Property

    Public Property SaleTypeID() As Integer
        Get
            Return _SaleTypeID
        End Get
        Set(ByVal value As Integer)
            _SaleTypeID = value
        End Set
    End Property

    Public Property SaleSubTypeID() As Integer
        Get
            Return _SaleSubType
        End Get
        Set(ByVal value As Integer)
            _SaleSubType = value
        End Set
    End Property

    Public Property FundingStatusID() As Integer
        Get
            Return _FundingStatusID
        End Get
        Set(ByVal value As Integer)
            _FundingStatusID = value
        End Set
    End Property

    Public Property FundingDate() As String
        Get
            Return _FundingDate
        End Get
        Set(ByVal value As String)
            _FundingDate = value
        End Set
    End Property

    Public Property AnniversaryDate() As String
        Get
            Return _AnniversaryDate
        End Get
        Set(ByVal value As String)
            _AnniversaryDate = value
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

    Public Property TrustName() As String
        Get
            Return _TrustName
        End Get
        Set(ByVal value As String)
            _TrustName = value
        End Set
    End Property

    Public Property TrustFlag() As Boolean
        Get
            Return _TrustFlag
        End Get
        Set(ByVal value As Boolean)
            _TrustFlag = value
        End Set
    End Property

    Public Property CompanyFlag() As Boolean
        Get
            Return _CompanyFlag
        End Get
        Set(ByVal value As Boolean)
            _CompanyFlag = value
        End Set
    End Property

    Public Property CompanyName() As String
        Get
            Return _CompanyName
        End Get
        Set(ByVal value As String)
            _CompanyName = value
        End Set
    End Property

    Public Property SplitMF() As Boolean
        Get
            Return _SplitMF
        End Get
        Set(ByVal value As Boolean)
            _SplitMF = value
        End Set
    End Property

    Public Property OriginallyWrittenDate As String
        Get
            Return _OriginallyWrittenDate
        End Get
        Set(ByVal value As String)
            _OriginallyWrittenDate = value
        End Set
    End Property

    Public Property MaintenanceFeeStatusID As Integer
        Get
            Return _MaintenanceFeeStatusID
        End Get
        Set(ByVal value As Integer)
            _MaintenanceFeeStatusID = value
        End Set
    End Property







End Class
