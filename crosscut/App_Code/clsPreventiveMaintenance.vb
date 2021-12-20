Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Threading


Public Class clsPreventiveMaintenance

    Public Sub New()
    End Sub

    Private _task As New clsPMTasks
    Private _item2track As New clsPMItems2Track
    Private _pmitem As New clsPMItems
    Private ds As DataSet

    
    Public ReadOnly Property TableSet As DataSet
        Get
            If Not ds Is Nothing Then Return ds
            ds = New DataSet()
            Using cn = New SqlConnection(Resources.Resource.cns)
                Dim sqlText = String.Format("select * from t_PMItems2Track order by name;select * from t_PMTasks where active=1;select * from t_PMItems;" & _
                                            "select * from t_PMBuilding;select * from t_Room order by charindex('-', roomnumber) asc, roomnumber;select * from t_combos;select * from t_comboitems;")

                Using ad = New SqlDataAdapter(sqlText, cn)
                    ad.TableMappings.Add("Table", "PMItems2Track")
                    ad.TableMappings.Add("Table1", "PMTasks")
                    ad.TableMappings.Add("Table2", "PMItems")
                    ad.TableMappings.Add("Table3", "PMBuilding")
                    ad.TableMappings.Add("Table4", "PMRoom")
                    ad.TableMappings.Add("Table5", "PMCombos")
                    ad.TableMappings.Add("Table6", "PMComboItems")

                    Dim cb = New SqlCommandBuilder(ad)
                    ad.MissingSchemaAction = MissingSchemaAction.AddWithKey
                    ad.FillSchema(ds, SchemaType.Source)
                    ad.Fill(ds)

                    Dim r_item2track_tasks = New DataRelation("item2track_tasks", ds.Tables("PMItems2Track").Columns("item2trackid"), ds.Tables("PMTasks").Columns("Item2TrackID"), False)
                    ds.Relations.Add(r_item2track_tasks)

                    Dim r_item2track_items = New DataRelation("item2track_items", ds.Tables("PMItems2Track").Columns("item2trackid"), ds.Tables("PMItems").Columns("item2trackid"), False)
                    ds.Relations.Add(r_item2track_items)

                    Dim r_building_items = New DataRelation("building_items", ds.Tables("PMBuilding").Columns("pmbuildingid"), ds.Tables("PMItems").Columns("keyvalue"), False)
                    ds.Relations.Add(r_building_items)

                    Dim r_room_items = New DataRelation("room_items", ds.Tables("PMRoom").Columns("roomid"), ds.Tables("PMItems").Columns("keyvalue"), False)
                    ds.Relations.Add(r_room_items)

                    Dim r_combo_comboitems = New DataRelation("combo_comboitems", ds.Tables("PMCombos").Columns("comboid"), ds.Tables("PMComboItems").Columns("comboid"), False)
                    ds.Relations.Add(r_combo_comboitems)

                End Using
            End Using
            Return ds
        End Get
    End Property

    Public Sub Dispose()
        If Not ds Is Nothing Then ds.Dispose()        
        ds = Nothing
    End Sub

    Public Function List_All() As SqlDataSource
        Return New SqlDataSource With { _
            .ConnectionString = Resources.Resource.cns, _
            .SelectCommand = "select s.ScheduleID, s.TaskID, s.PMItemID, s.DateCreated, s.ServiceDate, s.WODateGenerated, s.RequestID, i.KeyField, i.KeyValue, " & _
                                            "(select RoomNumber from t_Room r where r.RoomID = i.KeyValue and i.KeyField = 'roomid') room, " & _
                                            "(select name from t_PMBuilding where pmbuildingID = i.KeyValue and i.KeyField = 'pmbuildingid' ) building, " & _
                                            "i.Item2TrackID, i.Item2TrackID, i2t.name [item-track-name], t.Name [task-name] from t_PMSchedules s inner join t_PMItems i on s.PMItemID = i.PMItemID " & _
                                            "inner join t_PMItems2Track i2t on i.Item2TrackID = i2t.item2trackid inner join t_PMTasks t on s.TaskID = t.TaskID order by room, building;;"}
    End Function

    Public Function List(unique As Boolean) As SqlDataSource
        If unique Then
            Return New SqlDataSource With { _
           .ConnectionString = Resources.Resource.cns, _
           .SelectCommand = "select distinct s.TaskID, s.PMItemID, i.KeyField, i.KeyValue, " & _
                                           "(select RoomNumber from t_Room r where r.RoomID = i.KeyValue and i.KeyField = 'roomid') room, " & _
                                           "(select name from t_PMBuilding where pmbuildingID = i.KeyValue and i.KeyField = 'pmbuildingid' ) building, " & _
                                           "i.Item2TrackID, i.Item2TrackID, i2t.name [item-track-name], t.Name [task-name] from t_PMSchedules s inner join t_PMItems i on s.PMItemID = i.PMItemID " & _
                                           "inner join t_PMItems2Track i2t on i.Item2TrackID = i2t.item2trackid inner join t_PMTasks t on s.TaskID = t.TaskID order by room, building;;"}
        Else
            Return New SqlDataSource With { _
           .ConnectionString = Resources.Resource.cns, _
           .SelectCommand = "select s.ScheduleID, s.TaskID, s.PMItemID, s.DateCreated, s.ServiceDate, s.WODateGenerated, s.RequestID, i.KeyField, i.KeyValue, " & _
                                           "(select RoomNumber from t_Room r where r.RoomID = i.KeyValue and i.KeyField = 'roomid') room, " & _
                                           "(select name from t_PMBuilding where pmbuildingID = i.KeyValue and i.KeyField = 'pmbuildingid' ) building, " & _
                                           "i.Item2TrackID, i.Item2TrackID, i2t.name [item-track-name], t.Name [task-name] from t_PMSchedules s inner join t_PMItems i on s.PMItemID = i.PMItemID " & _
                                           "inner join t_PMItems2Track i2t on i.Item2TrackID = i2t.item2trackid inner join t_PMTasks t on s.TaskID = t.TaskID order by room, building;;"}
        End If
    End Function

    Public Function List_Items2Track() As SqlDataSource
        Dim ds = New SqlDataSource
        With ds
            .ConnectionString = Resources.Resource.cns
            .SelectCommand = "select * from t_pmItems2Track order by name"
        End With
        Return ds
    End Function

    Public Function GetTasks() As SqlDataSource
        Return New SqlDataSource With { _
            .ConnectionString = Resources.Resource.cns, _
            .SelectCommand = "select *, ti.comboitem as issue, case when t.active = 1 then 'Active' else 'Not Active' end [Task Status] from t_PMTasks t inner join t_ComboItems ti on t.IssueID = ti.ComboItemID order by t.Name;"}
    End Function

    Public Function GetItems2Track() As SqlDataSource
        Dim ds = New SqlDataSource
        With ds
            .ConnectionString = Resources.Resource.cns
            .SelectCommand = "select *, (select ComboItem from t_ComboItems where ComboItemID = i2t.categoryid) category from t_PMItems2Track i2t order by name;"
        End With
        Return ds
    End Function

    Public Function GetKCPUnits() As SqlDataSource
        Dim ds = New SqlDataSource
        With ds
            .ConnectionString = Resources.Resource.cns
            .SelectCommand = "select roomid ID, RoomNumber Name, 'roomid' Type from t_Room " & _
                                            " union all " & _
                                            "select pmbuildingID, name, 'pmbuildingid' from t_PMBuilding " & _
                                            "order by Type, name"
        End With
        Return ds
    End Function

    Public Function GetItem2TrackCategoryList() As SqlDataSource
        Dim ds = New SqlDataSource
        With ds
            .ConnectionString = Resources.Resource.cns
            .SelectCommand = "select * from t_ComboItems ci inner join t_Combos c on ci.ComboID = c.ComboID where c.ComboName = 'PreventiveMaintenance' order by ci.comboItem "
        End With
        Return ds
    End Function

    Public Function GetMaintenanceRequestIssueList() As SqlDataSource
        Dim ds = New SqlDataSource
        With ds
            .ConnectionString = Resources.Resource.cns
            .SelectCommand = "select * from t_ComboItems ci inner join t_Combos c on ci.ComboID = c.ComboID where c.ComboName = 'MaintenanceRequestIssue' order by ci.comboItem "
        End With
        Return ds
    End Function

    Public Function Get_Tasks_Table() As SqlDataSource        
        Return New SqlDataSource With { _
            .ConnectionString = Resources.Resource.cns, _
            .SelectCommand = "select *, case when t.active = 1 then 'Active' else 'Not Active' end [Task Status] from t_PMTasks t inner join t_ComboItems ti on t.IssueID = ti.ComboItemID order by t.Name;"}
    End Function

    Public Function List_Tasks() As SqlDataSource
        Dim ds = New SqlDataSource
        With ds
            .ConnectionString = Resources.Resource.cns
            .SelectCommand = "select * from t_pmTasks order by name"
        End With
        Return ds
    End Function
    Public ReadOnly Property Issues As DataTable
        Get
            Dim r = From c As DataRow In TableSet.Tables("PMCombos").AsEnumerable() _
                    Join ci As DataRow In TableSet.Tables("PMComboItems").AsEnumerable() On ci("comboid").ToString() Equals c("comboid").ToString() _
                    Where c("comboname").ToString().ToLower() = "MaintenanceRequestIssue".ToLower() _
                    Select ci
            Return r.ToArray().CopyToDataTable()
        End Get
    End Property
    Public ReadOnly Property Categories As DataTable
        Get
            Dim r = From c As DataRow In TableSet.Tables("PMCombos").AsEnumerable() _
                    Join ci As DataRow In TableSet.Tables("PMComboItems").AsEnumerable() On ci("comboid").ToString() Equals c("comboid").ToString() _
                    Where c("comboname").ToString().ToLower() = "PreventiveMaintenance".ToLower() _
                    Select ci
            Return r.ToArray().CopyToDataTable()
        End Get
    End Property

    Public Function List(Optional searchText As String = "") As DataTable

        Dim dt = New DataTable()
        If searchText.Length = 0 Then
            dt = TableSet.Tables("PMItems2Track")
            dt.Columns.Add("Category", GetType(String))
            For Each drRow As DataRow In dt.Rows
                Dim row = drRow
                drRow("Category") = TableSet.Tables("PMComboItems").AsEnumerable().Single(Function(x) x("comboitemid").ToString() = row("categoryid").ToString())("comboitem")
            Next
        Else
            Dim rows_search = From c As DataRow In TableSet.Tables("PMItems2Track").AsEnumerable() _
                    .Where(Function(x) x("name").ToString().ToLower().Contains(searchText))

            If rows_search.Count() > 0 Then
                dt = rows_search.CopyToDataTable()
                dt.Columns.Add("Category", GetType(String))
                For Each drRow As DataRow In dt.Rows
                    Dim row = drRow
                    drRow("Category") = TableSet.Tables("PMComboItems").AsEnumerable().Single(Function(x) x("comboitemid").ToString() = row("categoryid").ToString())("comboitem")
                Next
            End If
        End If
        Return dt
    End Function

    Public ReadOnly Property Rooms As DataTable
        Get
            If TableSet.Tables("PMRoom").Rows.Count = 0 Then
                Return New DataTable()
            Else
                Return (From c As DataRow In TableSet.Tables("PMRoom").Rows.OfType(Of DataRow).AsEnumerable()).ToArray().CopyToDataTable()
            End If
        End Get
    End Property
    Public ReadOnly Property Tasks As DataTable
        Get
            If TableSet.Tables("PMTasks").Rows.Count = 0 Then
                Return New DataTable()
            Else
                Return (From c As DataRow In TableSet.Tables("PMTasks").Rows.OfType(Of DataRow).OrderBy(Function(x) x("name").ToString()).AsEnumerable()).ToArray().CopyToDataTable()
            End If
        End Get
    End Property
    Public ReadOnly Property Buildings As DataTable
        Get
            If TableSet.Tables("PMBuilding").Rows.Count = 0 Then
                Return New DataTable()
            Else
                Return (From c As DataRow In TableSet.Tables("PMBuilding").Rows.OfType(Of DataRow).OrderBy(Function(x) x("name").ToString()).AsEnumerable()).ToArray().CopyToDataTable()
            End If
        End Get
    End Property
    Public ReadOnly Property Items As DataTable
        Get
            If TableSet.Tables("PMItems2Track").Rows.Count = 0 Then
                Return New DataTable()
            Else
                Return (From c As DataRow In TableSet.Tables("PMItems2Track").Rows.OfType(Of DataRow).OrderBy(Function(x) x("name").ToString()).AsEnumerable()).ToArray().CopyToDataTable()
            End If
        End Get
    End Property


    Public ReadOnly Property Departments As String()
        Get
            Return New String() {"housekeep", "maint", "mstrcorp"}
        End Get
    End Property
    Public ReadOnly Property Task As clsPMTasks
        Get
            Return _task
        End Get
    End Property
    Public ReadOnly Property Item2Track As clsPMItems2Track
        Get
            Return _item2track
        End Get
    End Property
    Public ReadOnly Property PMItem As clsPMItems
        Get
            Return _pmitem
        End Get
    End Property

    Public Class Item2TrackItems
        Inherits clsPreventiveMaintenance

        Public Function Count(item2trackID As Int32) As Int32
            Dim r_item2track_items = MyBase.TableSet.Relations("item2track_items")
            Dim parent = r_item2track_items.ParentTable
            Return parent.AsEnumerable() _
                .Where(Function(x) x("item2trackid").ToString() = item2trackID) _
                .Single().GetChildRows(r_item2track_items) _
                .Where(Function(x) x("keyfield").ToString() = "pmbuildingid").Count()
        End Function

        Public Shadows Function Buildings(item2trackID As Int32) As List(Of DataRow)
            Dim r_item2track_items = MyBase.TableSet.Relations("item2track_items")
            Dim parent = r_item2track_items.ParentTable
            Return parent.AsEnumerable() _
                .Where(Function(x) x("item2trackid").ToString() = item2trackID) _
                .Single().GetChildRows(r_item2track_items) _
                .Where(Function(x) x("keyfield").ToString() = "pmbuildingid").ToList()
        End Function

        Public Shadows Function Rooms(item2trackID As Int32) As List(Of DataRow)
            Dim r_item2track_items = MyBase.TableSet.Relations("item2track_items")
            Dim parent = r_item2track_items.ParentTable
            Return parent.AsEnumerable() _
                .Where(Function(x) x("item2trackid").ToString() = item2trackID) _
                .Single().GetChildRows(r_item2track_items) _
                .Where(Function(x) x("keyfield").ToString().ToLower() = "roomid").ToList()
        End Function
    End Class

    Public Class Item
        Inherits clsPreventiveMaintenance

        Public Function ToTrackItem(pmItemID As Int32, item2TrackID As Int32) As DataRow
            Dim r = MyBase.TableSet.Relations("item2track_items")
            Dim dt = r.ParentTable.AsEnumerable().Where(Function(x) x("item2TrackID").ToString() = item2TrackID).CopyToDataTable().Clone()
            dt.Columns.Add("DateAdded", GetType(String))
            dt.Columns.Add("DateRemoved", GetType(String))
            dt.Columns.Add("ExtraText", GetType(String))
            For Each dr As DataRow In r.ParentTable.AsEnumerable().Where(Function(x) x("item2TrackID").ToString() = item2TrackID)
                dt.ImportRow(dr)
                dt.Rows(dt.Rows.Count - 1)("DateAdded") = dr.GetChildRows(r).First(Function(x) x("pmItemID").ToString() = pmItemID)("dateadded").ToString()
                dt.Rows(dt.Rows.Count - 1)("DateRemoved") = dr.GetChildRows(r).First(Function(x) x("pmItemID").ToString() = pmItemID)("dateremoved").ToString()
                dt.Rows(dt.Rows.Count - 1)("ExtraText") = dr.GetChildRows(r).First(Function(x) x("pmItemID").ToString() = pmItemID)("description").ToString()
            Next
            Return dt.Rows(0)
        End Function
    End Class

    Public Class BuildingItems
        Inherits clsPreventiveMaintenance

        Public Shadows Function Name(keyvalue As Int32) As String
            Dim r_building_items = MyBase.TableSet.Relations("building_items")
            Dim child = r_building_items.ChildTable
            Return child.AsEnumerable() _
                .Where(Function(x) x("keyvalue").ToString() = keyvalue And x("keyfield") = "pmbuildingid") _
                .First().GetParentRow("building_items").Field(Of String)("Name")
        End Function
    End Class

    Public Class RoomItems
        Inherits clsPreventiveMaintenance

        Public Shadows Function RoomNumber(keyvalue As Int32) As String
            Dim r_room_items = MyBase.TableSet.Relations("room_items")
            Dim child = r_room_items.ChildTable
            Return child.AsEnumerable() _
                .Where(Function(x) x("keyvalue").ToString() = keyvalue And x("keyfield").ToString().ToLower() = "roomid") _
                .First().GetParentRow("room_items").Field(Of String)("RoomNumber")
        End Function
    End Class

    Public Class Item2TrackTasks
        Inherits clsPreventiveMaintenance

        Public Shadows Function List(item2trackID As Int32) As List(Of DataRow)
            Dim r_item2track_tasks = MyBase.TableSet.Relations("item2track_tasks")
            Dim parent = r_item2track_tasks.ParentTable
            Return parent.AsEnumerable() _
                .Where(Function(x) x("item2trackid").ToString() = item2trackID) _
                .Single().GetChildRows(r_item2track_tasks).ToList()
        End Function

        Public Shadows Function Name(issueID As Int32) As String
            Dim r_combo_comboitems = MyBase.TableSet.Relations("combo_comboitems")
            Dim child = r_combo_comboitems.ChildTable
            Return child.AsEnumerable().First(Function(x) x("comboitemid").ToString() = issueID)("comboitem")
        End Function
    End Class

    ''' <summary>
    ''' Retrieves all Item2TrackID for each room or building
    ''' </summary>
    Public Class Item2ItemTracks
        Inherits clsPreventiveMaintenance

        Public Shadows Function List(keyfield As String, keyvalue As Int32) As DataTable

            Dim r = MyBase.TableSet.Relations("item2track_items")
            Dim child = r.ChildTable           
            Dim dtParent As DataTable = r.ParentTable.Clone()
            dtParent.PrimaryKey = Nothing
            dtParent.Columns.Add("Category", GetType(String))
            dtParent.Columns.Add("ExtraText", GetType(String))
            dtParent.Columns.Add("PMItemID", GetType(String))
            dtParent.Columns.Add("DateAdded", GetType(String))
            dtParent.Columns.Add("DateRemoved", GetType(String))

            For Each drRow As DataRow In child.AsEnumerable().Where(Function(x) x("keyfield").ToString().ToLower() = keyfield.ToLower() And _
                                                  Convert.ToInt32(x("keyvalue").ToString()) = keyvalue)
                dtParent.ImportRow(drRow.GetParentRow(r))
                If dtParent.Rows.Count > 0 Then
                    dtParent.Rows(dtParent.Rows.Count - 1)("ExtraText") = drRow("description").ToString()
                    dtParent.Rows(dtParent.Rows.Count - 1)("PMItemID") = drRow("PMItemID").ToString()

                    If drRow("dateAdded").ToString.Length > 0 Then
                        dtParent.Rows(dtParent.Rows.Count - 1)("DateAdded") = Convert.ToDateTime(drRow("dateAdded").ToString()).ToShortDateString()
                    End If

                    If drRow("dateRemoved").ToString().Length > 0 Then
                        dtParent.Rows(dtParent.Rows.Count - 1)("DateRemoved") = Convert.ToDateTime(drRow("DateRemoved").ToString()).ToShortDateString()
                    End If

                End If
            Next
            For Each drRow As DataRow In dtParent.Rows
                Dim row = drRow
                drRow("Category") = MyBase.TableSet.Tables("PMComboItems").AsEnumerable().Single(Function(x) x("comboitemid").ToString() = row("categoryid").ToString())("comboitem")
            Next
            Return dtParent
        End Function

    End Class

    Public Class Schedule
        Inherits clsPreventiveMaintenance

        Private Sub Delete(item2trackID As Int32)

            Using cn = New SqlConnection(Resources.Resource.cns)
                Dim sqlText = String.Format( _
                               "delete from t_pmschedules where scheduleid in (select s.scheduleid from t_PMItems2Track i2t inner join t_PMTasks t on t.Item2TrackID = i2t.item2trackid " & _
                               "inner join t_PMItems i on i.Item2TrackID = i2t.item2trackid " & _
                               "inner join t_PMSchedules s on s.taskid = t.TaskID and s.pmitemid = i.PMItemID " & _
                               "where i2t.item2trackid = {0} And s.WODateGenerated is null and s.RequestID is null and s.WOGenerated=0)", item2trackID)
                Using cm = New SqlCommand(sqlText, cn)
                    Try
                        cn.Open()
                        cm.ExecuteNonQuery()
                    Catch ex As Exception
                        HttpContext.Current.Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End Sub

        Private Sub Create(item2trackID As Int32, pmitemID As Int32)
            Dim sb = New StringBuilder()
            Dim l As New List(Of String)

            Using cn = New SqlConnection(Resources.Resource.cns)
                Try
                    cn.Open()
                    Using cm = New SqlCommand()
                        cm.Connection = cn
                        If pmitemID = -1 Then
                            For Each itemRow As DataRow In MyBase.TableSet.Relations("item2track_items").ChildTable.AsEnumerable() _
                            .Where(Function(x) x("item2trackID").ToString() = item2trackID And x("dateremoved").Equals(DBNull.Value) = True)
                                For Each taskRow As DataRow In MyBase.TableSet.Relations("item2track_tasks").ChildTable.AsEnumerable().Where(Function(x) x("item2trackid").ToString() = item2trackID And Convert.ToBoolean(x("active").ToString()) = True)

                                    Dim lifeSpan As Int32 = taskRow.GetParentRow("item2track_tasks")("life")
                                    Dim startDate = Convert.ToDateTime(itemRow("dateadded").ToString())
                                    Dim interval As Int32 = taskRow("interval")
                                    Dim endDate As DateTime = New DateTime(startDate.Year, startDate.Month, startDate.Day).AddMonths(lifeSpan)
                                    Dim months = DateDiff(DateInterval.Month, startDate, endDate)
                                    Dim increment As Int32 = months / interval

                                    For i = 1 To increment
                                        Dim nextDate As DateTime = startDate.AddMonths(interval * i)
                                        l.Add(String.Format("({0},{1},getdate(),'{2}',0,1)", taskRow("taskid").ToString(), itemRow("pmItemID").ToString(), nextDate))
                                    Next
                                Next
                            Next

                            For i = 0 To l.Count - 1 Step 1000
                                Dim arCopy = l.Skip(i).Take(1000)
                                cm.CommandText = String.Format("insert into t_pmschedules(TaskID, PMItemID, DateCreated, ServiceDate, WOGenerated, Times) values {0}", String.Join(",", arCopy.ToArray))
                                cm.ExecuteNonQuery()
                            Next

                        Else
                            For Each itemRow As DataRow In MyBase.TableSet.Relations("item2track_items").ChildTable.AsEnumerable() _
                                .Where(Function(x) x("item2trackID").ToString() = item2trackID And x("dateremoved").Equals(DBNull.Value) = True _
                                And x("pmitemid").ToString() = pmitemID)


                                For Each taskRow As DataRow In MyBase.TableSet.Relations("item2track_tasks").ChildTable.AsEnumerable().Where(Function(x) x("item2trackid").ToString() = item2trackID And Convert.ToBoolean(x("active").ToString()) = True)

                                    Dim lifeSpan As Int32 = taskRow.GetParentRow("item2track_tasks")("life")
                                    Dim startDate = Convert.ToDateTime(itemRow("dateadded").ToString())
                                    Dim interval As Int32 = taskRow("interval")
                                    Dim endDate As DateTime = New DateTime(startDate.Year, startDate.Month, startDate.Day).AddMonths(lifeSpan)
                                    Dim months = DateDiff(DateInterval.Month, startDate, endDate)
                                    Dim increment As Int32 = months / interval

                                    For i = 1 To increment
                                        Dim nextDate As DateTime = startDate.AddMonths(interval * i)
                                        l.Add(String.Format("({0},{1},getdate(),'{2}',0,1)", taskRow("taskid").ToString(), itemRow("pmItemID").ToString(), nextDate))
                                    Next
                                Next
                            Next

                            For i = 0 To l.Count - 1 Step 1000
                                Dim arCopy = l.Skip(i).Take(1000)
                                cm.CommandText = String.Format("insert into t_pmschedules(TaskID, PMItemID, DateCreated, ServiceDate, WOGenerated, Times) values {0}", String.Join(",", arCopy.ToArray))
                                cm.ExecuteNonQuery()
                            Next

                        End If

                    End Using
                Catch ex As Exception
                    HttpContext.Current.Response.Write(ex.Message)
                Finally
                    cn.Close()
                End Try

            End Using
        End Sub

        Public Sub UpdateTask(taskID As Int32, item2trackID As Int32)
            Dim sb = New StringBuilder()
            Dim l As New List(Of String)

            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("delete from t_pmschedules where taskid={0} and wodategenerated is null and requestid is null and WOgenerated=0", taskID), cn)
                    Try
                        cn.Open()
                        cm.ExecuteNonQuery()

                        For Each itemRow As DataRow In MyBase.TableSet.Relations("item2track_items").ChildTable.AsEnumerable() _
                            .Where(Function(x) x("dateremoved").ToString().Length = 0 And x("item2trackID").ToString() = item2trackID)

                            cm.CommandText = String.Format("select top 1 WODateGenerated from t_pmschedules where taskid={0} and pmitemid={1} " & _
                                                           "and wodategenerated is not null and requestid is not null and WOgenerated=1 order by scheduleID desc", taskID, itemRow("pmitemid").ToString())
                            Dim oDate As Object = cm.ExecuteScalar()
                            Dim startDate As DateTime = DateTime.MaxValue

                            If Not oDate Is Nothing Then startDate = Convert.ToDateTime(oDate)

                            For Each taskRow As DataRow In MyBase.TableSet.Relations("item2track_tasks").ChildTable.AsEnumerable() _
                                .Where(Function(x) x("taskID").ToString() = taskID And Convert.ToBoolean(x("active").ToString()) = True)

                                Dim lifeSpan As Int32 = taskRow.GetParentRow("item2track_tasks")("life")
                                If oDate Is Nothing Then startDate = Convert.ToDateTime(itemRow("dateadded").ToString())
                                Dim dateAdded As DateTime = Convert.ToDateTime(itemRow("dateadded").ToString())

                                Dim interval As Int32 = taskRow("interval")
                                Dim endDate As DateTime = New DateTime(startDate.Year, startDate.Month, startDate.Day).AddMonths(lifeSpan)

                                Dim months = DateDiff(DateInterval.Month, startDate, endDate)
                                Dim increment As Int32 = months / interval
                                For i = 1 To increment
                                    Dim nextDate As DateTime = startDate.AddMonths(interval * i)
                                    If nextDate.CompareTo(dateAdded.AddMonths(lifeSpan)) <= 0 Then
                                        l.Add(String.Format("({0},{1},getdate(),'{2}',0,1)", taskRow("taskid").ToString(), itemRow("pmItemID").ToString(), nextDate))
                                    End If
                                Next
                            Next
                        Next
                        For i = 0 To l.Count - 1 Step 1000
                            Dim arCopy = l.Skip(i).Take(1000)
                            cm.CommandText = String.Format("insert into t_pmschedules(TaskID, PMItemID, DateCreated, ServiceDate, WOGenerated, Times) values {0}", String.Join(",", arCopy.ToArray))
                            cm.ExecuteNonQuery()
                        Next
                    Catch ex As Exception
                        HttpContext.Current.Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End Sub

        Public Sub CreateTask(item2trackID As Int32, pmitemID As Int32)
            Create(item2trackID, pmitemID)
        End Sub

        Public Sub CreateItem2Track(item2trackID As Int32, pmitemID As Int32)
            Create(item2trackID, pmitemID)
        End Sub

        Public Sub UpdateItem2Track(item2trackID As Int32, pmitemID As Int32)
            Delete(item2trackID)
            Create(item2trackID, pmitemID)
        End Sub

        Public Sub RemoveTask(taskID As Int32)
            Using cn = New SqlConnection(Resources.Resource.cns)
                Dim sqlText = String.Format( _
                               "delete from t_pmschedules where wogenerated=0 and times=1 and wodategenerated is null and requestid is null and taskid={0}", taskID)
                Using cm = New SqlCommand(sqlText, cn)
                    Try
                        cn.Open()
                        cm.ExecuteNonQuery()
                    Catch ex As Exception
                        HttpContext.Current.Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End Sub

        ''' <summary>
        ''' Integer parameters are in months
        ''' </summary>
        Public Function Create_Schedules(lifeSpan As Integer, taskInterval As Integer, dateAdded As DateTime) As List(Of DateTime)
            Dim list As New List(Of DateTime)
            Dim date_end = dateAdded.AddMonths(lifeSpan)
            Dim months_between = DateDiff(DateInterval.Month, dateAdded, date_end)
            Dim months_increment = months_between / taskInterval
            For i As Integer = 1 To months_increment
                Dim next_date As DateTime = dateAdded.AddMonths(taskInterval * i)
                list.Add(next_date)
            Next
            Return list
        End Function

        Public Function Recreate_Schedules(lifeSpan As Integer, taskInterval As Integer, dateAdded As DateTime, dateChanged As DateTime) As List(Of DateTime)
            Dim schedules_old = Create_Schedules(lifeSpan, taskInterval, dateAdded)
            Dim schedules_new = Create_Schedules(lifeSpan, taskInterval, dateChanged)
            Dim schedules_last As DateTime = schedules_old.Last()
            Dim list = schedules_new.Where(Function(x) x.CompareTo(dateChanged) > 0 And x.CompareTo(schedules_last) <= 0).ToList()
            Return list
        End Function

        Public Function Last_Schedule_Completed(taskID As Integer, pmItemID As Integer) As DateTime
            Dim dt = Get_Schedules(taskID, pmItemID)
            Dim date_row = dt.AsEnumerable().Where(Function(x) x("requestid").Equals(DBNull.Value) = False And x("wogenerated").ToString() = True And x("wodategenerated").Equals(DBNull.Value) = False).OrderBy(Function(x) Convert.ToDateTime(x("servicedate").ToString())).LastOrDefault()
            If date_row Is Nothing Then
                Return DateTime.MinValue
            Else
                Return Convert.ToDateTime(date_row("servicedate").ToString())
            End If
        End Function

        Public Function Last_Schedule_Completed(pmItemID As Integer) As DateTime
            Dim dt = Get_Schedules(pmItemID)
            Dim date_row = dt.AsEnumerable().Where(Function(x) x("requestid").Equals(DBNull.Value) = False And x("wogenerated").ToString() = True And x("wodategenerated").Equals(DBNull.Value) = False).OrderBy(Function(x) Convert.ToDateTime(x("servicedate").ToString())).LastOrDefault()
            If date_row Is Nothing Then
                Return DateTime.MinValue
            Else
                Return Convert.ToDateTime(date_row("servicedate").ToString())
            End If
        End Function


        ''' <summary>
        ''' Get the life span, the interval, date created and date removed by the Item2TrackID returned as a string array
        ''' </summary>       
        Public Function Item_Dates(item2TrackID As Integer, taskID As Integer, pmItemID As Integer) As String()
            Dim ds = TableSet
            Dim s = From a As DataRow In ds.Tables("PMItems2Track") _
                    Join b As DataRow In ds.Tables("PMItems") On a("item2trackid").ToString() Equals b("item2trackid").ToString() _
                    Join c As DataRow In ds.Tables("PMTasks") On b("item2trackid").ToString() Equals c("item2trackid").ToString() _
                    Where a("item2trackid").ToString() = item2TrackID And b("pmitemid").ToString() = pmItemID And c("taskid").ToString() = taskID _
                    Select New String() {a("life"), c("interval").ToString(), b("dateadded").ToString, b("dateremoved").ToString()}
            Return s.Single()
        End Function

        ''' <summary>
        ''' Returns the Item2TrackID, pass in either taskID or pmItemID as parameter
        ''' </summary>
        Public Function Get_Item2TrackID(Optional taskID As Integer = 0, Optional PMItemID As Int16 = 0) As Integer
            Dim ds = TableSet
            Dim item2track_id = 0
            If taskID > 0 Then
                item2track_id = (From a As DataRow In ds.Tables("PMTasks") Where a("taskid").ToString() = taskID).First()("item2trackid").ToString()
            ElseIf PMItemID > 0 Then
                item2track_id = (From a As DataRow In ds.Tables("PMItems") Where a("pmitemid").ToString() = PMItemID).First()("item2trackid").ToString()
            End If
            Return item2track_id
        End Function


        ''' <summary>
        ''' Returns all schedules by taskID and pmItemID parameters
        ''' </summary>
        Public Function Get_Schedules(taskID As Integer, pmItemID As Integer) As DataTable
            Dim dt = New DataTable()
            Using cn = New SqlConnection(Resources.Resource.cns)
                Dim sqlText = String.Format( _
                               "select * from t_pmschedules where taskid={0} and pmitemid={1}", taskID, pmItemID)
                Using cm = New SqlCommand(sqlText, cn)
                    Try
                        cn.Open()
                        Dim rdr = cm.ExecuteReader()
                        dt.Load(rdr)
                    Catch ex As Exception
                        HttpContext.Current.Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return dt
        End Function

        Public Function Get_Schedules(pmItemID As Integer) As DataTable
            Dim dt = New DataTable()
            Using cn = New SqlConnection(Resources.Resource.cns)
                Dim sqlText = String.Format( _
                               "select * from t_pmschedules where pmitemid={0}", pmItemID)
                Using cm = New SqlCommand(sqlText, cn)
                    Try
                        cn.Open()
                        Dim rdr = cm.ExecuteReader()
                        dt.Load(rdr)
                    Catch ex As Exception
                        HttpContext.Current.Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return dt
        End Function


        ''' <summary>
        ''' Deletes schedules given an array of ID
        ''' </summary>        
        Public Sub Delete_Schedules(scheduleID() As String)
            Using cn = New SqlConnection(Resources.Resource.cns)
                Dim sqlText = String.Format( _
                               "delete from t_pmschedules where scheduleid in ({0})", String.Join(",", scheduleID))
                Using cm = New SqlCommand(sqlText, cn)
                    Try
                        cn.Open()
                        cm.ExecuteNonQuery()
                    Catch ex As Exception
                        HttpContext.Current.Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End Sub


        ''' <summary>
        ''' Get TaskID and PMItemID in a string array
        ''' </summary>       
        Public Function Schedule_By_RequestID(requestID As Integer) As String()
            Dim s() As String = New String() {"", "", ""}
            Using cn = New SqlConnection(Resources.Resource.cns)
                Dim sqlText = String.Format( _
                               "select top 1 * from t_pmschedules where requestid={0} order by scheduleid desc", requestID)
                Using cm = New SqlCommand(sqlText, cn)
                    Try
                        cn.Open()
                        Dim rdr = cm.ExecuteReader()
                        Dim dt = New DataTable()
                        dt.Load(rdr)
                        If dt.Rows.Count = 1 Then
                            s(0) = dt.Rows(0)("scheduleid").ToString()
                            s(1) = dt.Rows(0)("taskid").ToString()
                            s(2) = dt.Rows(0)("pmitemid").ToString()
                        End If
                    Catch ex As Exception
                        HttpContext.Current.Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            Return s
        End Function

        Private Sub Change_Service_Date(scheduleID As Integer, serviceDate As DateTime)
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("update t_pmschedules set servicedate='{0}' where scheduleid={1}", serviceDate, scheduleID), cn)
                    Try
                        cn.Open()
                        cm.ExecuteNonQuery()
                    Catch ex As Exception
                        HttpContext.Current.Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End Sub

        Private Sub Schedule_Reset(scheduleid() As String)
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(String.Format("update t_pmschedules set times=0 where scheduleid in ({0})", String.Join(",", scheduleid)), cn)
                    Try
                        cn.Open()
                        cm.ExecuteNonQuery()
                    Catch ex As Exception
                        HttpContext.Current.Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End Sub

        ''' <summary>
        ''' Deletes all schedules since the last one (request with work order), then re-create new schedules
        ''' </summary>
        Public Sub Schedule_Update(requestID As Integer)
            Dim task_id = 0
            Dim pmitem_id = 0
            Dim schedule_id = 0

            Dim schedule_detail = Schedule_By_RequestID(requestID)
            'HttpContext.Current.Response.Write(String.Format("<br/>{0}", schedule_detail(0)))

            schedule_id = schedule_detail(0)
            task_id = schedule_detail(1)
            pmitem_id = schedule_detail(2)

            If schedule_detail(0) = "" Then Return
            Dim schedule_complete_last = Last_Schedule_Completed(task_id, pmitem_id)
            'HttpContext.Current.Response.Write(String.Format("<br/>{0}", schedule_complete_last))
            If schedule_complete_last = DateTime.MinValue Then Return
            Dim schedule_dt = Get_Schedules(task_id, pmitem_id)

            'For Each dr As DataRow In schedule_dt.Rows.OfType(Of DataRow).Where(Function(x) Convert.ToDateTime(x("servicedate").ToString()).CompareTo(schedule_complete_last) > 0 And x("requestid").Equals(DBNull.Value) = True)
            '    HttpContext.Current.Response.Write(String.Format("<br/>{0}", dr("scheduleid").ToString()))
            'Next

            If schedule_dt.Rows.OfType(Of DataRow).Where(Function(x) Convert.ToDateTime(x("servicedate").ToString()).CompareTo(schedule_complete_last) > 0 And x("requestid").Equals(DBNull.Value) = True).Count() > 0 Then
                Delete_Schedules(schedule_dt.Rows.OfType(Of DataRow).Where(Function(x) Convert.ToDateTime(x("servicedate").ToString()).CompareTo(schedule_complete_last) > 0 And x("requestid").Equals(DBNull.Value) = True).Select(Function(x) x("scheduleid").ToString()).ToArray())
            End If

            Dim item2trackid = Get_Item2TrackID(schedule_detail(1))
            Dim dates_detail = Item_Dates(item2trackid, task_id, pmitem_id)
            Dim lifeSpan = dates_detail(0)
            Dim taskInterval = dates_detail(1)
            Dim dateAdded As DateTime = Convert.ToDateTime(dates_detail(2))
            Dim schedules_list = Recreate_Schedules(lifeSpan, taskInterval, dateAdded, DateTime.Now)

            If schedules_list.Count > 0 Then
                Using cn = New SqlConnection(Resources.Resource.cns)
                    Dim l As New List(Of String)
                    For Each dt As DateTime In schedules_list
                        'HttpContext.Current.Response.Write(String.Format("<br/>{0}", dt.ToShortDateString()))
                        l.Add(String.Format("({0},{1},getdate(),'{2}',0,1)", task_id, pmitem_id, dt))
                    Next

                    Try
                        Dim cm = New SqlCommand()
                        cm.Connection = cn
                        cn.Open()
                        For i = 0 To l.Count - 1 Step 1000
                            Dim arCopy = l.Skip(i).Take(1000)
                            cm.CommandText = String.Format("insert into t_pmschedules(TaskID, PMItemID, DateCreated, ServiceDate, WOGenerated, Times) values {0}", String.Join(",", arCopy.ToArray))
                            cm.ExecuteNonQuery()
                        Next
                    Catch ex As Exception
                        HttpContext.Current.Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using

            End If
            Change_Service_Date(schedule_detail(0), DateTime.Now)
        End Sub


        Public Sub Schedule_Create()

        End Sub

        Public Sub Schedule_Remove(pmItemID As Integer)
            Dim schedule_dt = Get_Schedules(pmItemID)           
            For Each gp As IGrouping(Of String, DataRow) In schedule_dt.Rows.OfType(Of DataRow).GroupBy(Function(x) x("taskid").ToString())
                Dim taskID = gp.Key
                Dim schedule_complete_last = Last_Schedule_Completed(taskID, pmItemID)

                Dim schedules = schedule_dt.Rows.OfType(Of DataRow).Where(Function(x) Convert.ToDateTime(x("servicedate").ToString()).CompareTo(schedule_complete_last) > 0 And x("requestid").Equals(DBNull.Value) = True And Convert.ToBoolean(x("wogenerated").ToString()) = False And x("wodategenerated").Equals(DBNull.Value) = True And x("taskid").ToString() = taskID And x("pmitemid").ToString() = pmItemID)
                If schedule_dt.Rows.OfType(Of DataRow).Except(schedules).Count() > 0 Then
                    Schedule_Reset(schedule_dt.Rows.OfType(Of DataRow).Except(schedules).Select(Function(x) x("scheduleid").ToString()).ToArray())
                End If

                If schedules.Count() > 0 Then
                    Delete_Schedules(schedules.Select(Function(x) x("scheduleid").ToString()).ToArray())
                End If
            Next
        End Sub

        Public Sub Schedule_Update(taskID As Integer, pmItemID As Integer, newDate As DateTime)
            Dim schedule_complete_last = Last_Schedule_Completed(taskID, pmItemID)
            Dim schedule_dt = Get_Schedules(taskID, pmItemID)
            If schedule_dt.Rows.OfType(Of DataRow).Where(Function(x) Convert.ToDateTime(x("servicedate").ToString()).CompareTo(schedule_complete_last) > 0 And x("requestid").Equals(DBNull.Value) = True).Count() > 0 Then
                Delete_Schedules(schedule_dt.Rows.OfType(Of DataRow).Where(Function(x) Convert.ToDateTime(x("servicedate").ToString()).CompareTo(schedule_complete_last) > 0 And x("requestid").Equals(DBNull.Value) = True).Select(Function(x) x("scheduleid").ToString()).ToArray())
            End If
            Dim item2trackid = Get_Item2TrackID(taskID)
            Dim dates_detail = Item_Dates(item2trackid, taskID, pmItemID)
            Dim lifeSpan = dates_detail(0)
            Dim taskInterval = dates_detail(1)
            Dim dateAdded As DateTime = Convert.ToDateTime(dates_detail(2))
            Dim schedules_list = Recreate_Schedules(lifeSpan, taskInterval, dateAdded, DateTime.Now)

            If schedules_list.Count > 0 Then
                Using cn = New SqlConnection(Resources.Resource.cns)
                    Dim l As New List(Of String)
                    For Each dt As DateTime In schedules_list
                        'HttpContext.Current.Response.Write(String.Format("<br/>{0}", dt.ToShortDateString()))
                        l.Add(String.Format("({0},{1},getdate(),'{2}',0,1)", taskID, pmItemID, newDate))
                    Next

                    Try
                        Dim cm = New SqlCommand()
                        cm.Connection = cn
                        cn.Open()
                        For i = 0 To l.Count - 1 Step 1000
                            Dim arCopy = l.Skip(i).Take(1000)
                            cm.CommandText = String.Format("insert into t_pmschedules(TaskID, PMItemID, DateCreated, ServiceDate, WOGenerated, Times) values {0}", String.Join(",", arCopy.ToArray))
                            cm.ExecuteNonQuery()
                        Next
                    Catch ex As Exception
                        HttpContext.Current.Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End If
        End Sub
    End Class
End Class
