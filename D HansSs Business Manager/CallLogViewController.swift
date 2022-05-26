//
//  MasterViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit
import CoreData

class CallLogViewController: UITableViewController, NSFetchedResultsControllerDelegate { //}, UISearchResultsUpdating {
    var detailViewController: CallLogDetailViewController? = nil
    var managedObjectContext: NSManagedObjectContext? = nil
    
//    let searchController = UISearchController(searchResultsController: nil)
//    var searchPredicate: NSPredicate? = nil
    var filteredObjects : [CallLog]? = nil
    var callTypeIcon: [UIImage] = [ #imageLiteral(resourceName: "ConnectUpIcon"), #imageLiteral(resourceName: "ProspectingIcon"), #imageLiteral(resourceName: "InformationIcon"), #imageLiteral(resourceName: "InviteIcon")]

    
//    func filterContentForSearchText(searchText: String, scope: String = "All" )
//    {
//        searchPredicate = NSPredicate(format: "callProspectName contains[c] %@", searchText)
//        filteredObjects = self.fetchedResultsController.fetchedObjects?.filter() {
//            return self.searchPredicate!.evaluate(with: $0)
//            } as [CallLog]?
//        self.tableView.reloadData()
//    }
//    func updateSearchResults(for searchController: UISearchController) {
//        filterContentForSearchText(searchText: searchController.searchBar.text!)
//    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        navigationItem.leftBarButtonItem = editButtonItem

        if let split = splitViewController {
            let controllers = split.viewControllers
            detailViewController = (controllers[controllers.count-1] as! UINavigationController).topViewController as? CallLogDetailViewController
        }

        // UISearchController setup
//        searchController.dimsBackgroundDuringPresentation = false
//        searchController.searchResultsUpdater = self
//        self.tableView.tableHeaderView = searchController.searchBar
//        self.definesPresentationContext = true
    }
    
    override func viewWillAppear(_ animated: Bool) {
        clearsSelectionOnViewWillAppear = splitViewController!.isCollapsed
        super.viewWillAppear(animated)
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    
    // MARK: - Segues
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if segue.identifier == "showCallLogDetail" {
            if let indexPath = tableView.indexPathForSelectedRow {
                var object : CallLog? = nil
//                if searchController.isActive && searchController.searchBar.text != ""
//                {
//                    object = filteredObjects?[indexPath.row]
//                }
//                else{
                    object = fetchedResultsController.object(at: indexPath)
//                }
                let controller = (segue.destination as! UINavigationController).topViewController as! CallLogDetailViewController
                controller.detailItem = object
                controller.navigationItem.leftBarButtonItem = splitViewController?.displayModeButtonItem
                controller.navigationItem.leftItemsSupplementBackButton = true
                controller.indexPath = indexPath
            }
        }
        if segue.identifier == "addCallLogSegue" {
            // LOAD NAMELIST im memory

        }
    }
    
    
    // MARK: - Table View
    
    override func numberOfSections(in tableView: UITableView) -> Int {
        return fetchedResultsController.sections?.count ?? 0
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
//        if searchController.isActive && searchController.searchBar.text != ""{
//            return (filteredObjects?.count)!
//        }
        let sectionInfo = fetchedResultsController.sections![section]
        return sectionInfo.numberOfObjects
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "CallLogCell", for: indexPath) as! CallLogTableViewCell
        
        var callLog : CallLog? = nil
//        if searchController.isActive && searchController.searchBar.text != ""{
//            callLog = filteredObjects?[indexPath.row]
//        }else{
            callLog = fetchedResultsController.object(at: indexPath)
//        }
        configureCell(cell, withName: callLog!)
//
//        cell.callInfoButton.tag = indexPath.row
//        cell.callInfoButton.addTarget(self, action: "showCallLogDetails", for: UIControlEvents.touchUpInside)
        return cell
    }
    
//    @IBAction func showCallLogDetails (sender : UIButton){
//        if let indexPath = tableView.indexPathForSelectedRow {
//            var object : CallLog? = nil
//            if searchController.isActive && searchController.searchBar.text != ""
//            {
//                object = filteredObjects?[indexPath.row]
//            }
//            else{
//                object = fetchedResultsController.object(at: indexPath)
//            }
//
//
//            let controller = (segue.destination as! UINavigationController).topViewController as! CallLogDetailViewController
//            controller.detailItem = object
//            controller.navigationItem.leftBarButtonItem = splitViewController?.displayModeButtonItem
//            controller.navigationItem.leftItemsSupplementBackButton = true
//            controller.indexPath = indexPath
//        }
//    }
    override func tableView(_ tableView: UITableView, canEditRowAt indexPath: IndexPath) -> Bool {
        // Return false if you do not want the specified item to be editable.
        return true
    }
    
    override func tableView(_ tableView: UITableView, commit editingStyle: UITableViewCellEditingStyle, forRowAt indexPath: IndexPath) {
        if editingStyle == .delete {
            let context = fetchedResultsController.managedObjectContext
            context.delete(fetchedResultsController.object(at: indexPath))
            
            do {
                try context.save()
            } catch {
                // Replace this implementation with code to handle the error appropriately.
                // fatalError() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development.
                let nserror = error as NSError
                fatalError("Unresolved error \(nserror), \(nserror.userInfo)")
            }
        }
    }
    
    func configureCell(_ cell: CallLogTableViewCell, withName callLog: CallLog) {
        
        cell.prospectNameTextLabel!.text = callLog.callProspectName!.description

        cell.callTimeTextLabel?.text = getDateText(callLog.callTime!)
        cell.callDetailsTextLabel?.text = callLog.callRemark
        cell.callTypeImage.image = callTypeIcon[Int(callLog.callType)]
        //TODO - save image saving of default type again and again
    }
    
    func getDateText(_ dateValue: Date) -> String {
        var date = ""
        let lastWeek = Calendar.current.date(byAdding: .day, value: -8 , to: Date())
        let calendar = NSCalendar.autoupdatingCurrent
//        let someDate: NSDate = dateValue as NSDate
        if calendar.isDateInToday(dateValue) {
            let dateFromat = DateFormatter()
            dateFromat.dateFormat = "hh:mm a"
            date = dateFromat.string(from: dateValue)
        }
        else if calendar.isDateInYesterday(dateValue) {
            date = "Yesterday"
        }
        else if (dateValue > lastWeek!) {
            let dateFromat = DateFormatter()
            dateFromat.dateFormat = "EEEE"
            date = dateFromat.string(from: dateValue)
        }
        else{
            let dateFromat = DateFormatter()
            dateFromat.dateFormat = "dd/MM/YY"
            date = dateFromat.string(from: dateValue)
        }
        
        return date
    }
    
    func dayDifference(from interval : TimeInterval) -> String
    {
        let calendar = NSCalendar.current
        let date = Date(timeIntervalSince1970: interval)
        if calendar.isDateInYesterday(date) { return "Yesterday" }
        else if calendar.isDateInToday(date) { return "Today" }
        else if calendar.isDateInTomorrow(date) { return "Tomorrow" }
        else {
            let startOfNow = calendar.startOfDay(for: Date())
            let startOfTimeStamp = calendar.startOfDay(for: date)
            let components = calendar.dateComponents([.day], from: startOfNow, to: startOfTimeStamp)
            let day = components.day!
            if day < 1 { return "\(abs(day)) days ago" }
            else { return "In \(day) days" }
        }
    }
    // MARK: - Fetched results controller
    
    var fetchedResultsController: NSFetchedResultsController<CallLog> {
        if _fetchedResultsController != nil {
            return _fetchedResultsController!
        }

        let fetchRequest: NSFetchRequest<CallLog> = CallLog.fetchRequest()

        // Set the batch size to a suitable number.
        fetchRequest.fetchBatchSize = 20

        // Edit the sort key as appropriate.
        let sortDescriptor = NSSortDescriptor(key: "callTime", ascending: false)

        fetchRequest.sortDescriptors = [sortDescriptor]

        // Edit the section name key path and cache name if appropriate.
        // nil for section name key path means "no sections".
        let aFetchedResultsController = NSFetchedResultsController(fetchRequest: fetchRequest, managedObjectContext: self.managedObjectContext!, sectionNameKeyPath: nil, cacheName: "CallLog")

        aFetchedResultsController.delegate = self
        _fetchedResultsController = aFetchedResultsController

        do {
            try _fetchedResultsController!.performFetch()
        } catch {
            // Replace this implementation with code to handle the error appropriately.
            // fatalError() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development.
            let nserror = error as NSError
            fatalError("Unresolved error \(nserror), \(nserror.userInfo)")
        }

        return _fetchedResultsController!
    }
    var _fetchedResultsController: NSFetchedResultsController<CallLog>? = nil
    
    func controllerWillChangeContent(_ controller: NSFetchedResultsController<NSFetchRequestResult>) {
        tableView.beginUpdates()
    }
    
    func controller(_ controller: NSFetchedResultsController<NSFetchRequestResult>, didChange sectionInfo: NSFetchedResultsSectionInfo, atSectionIndex sectionIndex: Int, for type: NSFetchedResultsChangeType) {
        switch type {
        case .insert:
            tableView.insertSections(IndexSet(integer: sectionIndex), with: .fade)
        case .delete:
            tableView.deleteSections(IndexSet(integer: sectionIndex), with: .fade)
        default:
            return
        }
    }
    
    func controller(_ controller: NSFetchedResultsController<NSFetchRequestResult>, didChange anObject: Any, at indexPath: IndexPath?, for type: NSFetchedResultsChangeType, newIndexPath: IndexPath?) {
        switch type {
        case .insert:
            tableView.insertRows(at: [newIndexPath!], with: .fade)
        case .delete:
            tableView.deleteRows(at: [indexPath!], with: .fade)
        case .update:
            configureCell(tableView.cellForRow(at: indexPath!)! as! CallLogTableViewCell, withName: anObject as! CallLog)
        case .move:
            configureCell(tableView.cellForRow(at: indexPath!)! as! CallLogTableViewCell, withName: anObject as! CallLog)
            tableView.moveRow(at: indexPath!, to: newIndexPath!)
        }
    }
    
    func controllerDidChangeContent(_ controller: NSFetchedResultsController<NSFetchRequestResult>) {
        tableView.endUpdates()
    }
    

    func save(callProspectName:String, callDetails:String, callType:Int, callTime: Date) {
        let context = self.fetchedResultsController.managedObjectContext
        let newCallLog = CallLog(context: context)
        // If appropriate, configure the new managed object.
        newCallLog.callProspectName = callProspectName
        newCallLog.callRemark = callDetails
        newCallLog.callType = Int32(callType)
        newCallLog.callTime = callTime

        // Save the context.
        do {
            try context.save()
        } catch {
            // Replace this implementation with code to handle the error appropriately.
            // fatalError() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development.
            let nserror = error as NSError
            fatalError("Unresolved error \(nserror), \(nserror.userInfo)")
        }
    }
    
    
    func update(indexPath: IndexPath, callProspectName:String, callDetails:String, callType:Int, callTime: Date) {

        let context = self.fetchedResultsController.managedObjectContext
        fetchedResultsController.object(at: indexPath).callProspectName = callProspectName
        fetchedResultsController.object(at: indexPath).callRemark = callDetails
        fetchedResultsController.object(at: indexPath).callType = Int32(callType)
        fetchedResultsController.object(at: indexPath).callTime = callTime

        do {

            try context.save()
        } catch {
            // Replace this implementation with code to handle the error appropriately.
            // fatalError() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development.
            let nserror = error as NSError
            fatalError("Unresolved error \(nserror), \(nserror.userInfo)")
        }
    }


    @IBAction func unwindToCallLog(segue: UIStoryboardSegue) {
        if let viewController = segue.source as? AddCallLogViewController {
            guard let callProspectName: String = viewController.callProspectNameTextField.text,
                let callDetails: String = viewController.callDetailsTextField.text,
                let callType: Int = viewController.callTypeSegmentedControl.selectedSegmentIndex,
                let callTime: Date = viewController.callDatePicker.date
                else { return }

            if callProspectName != "" && callDetails != "" {
                if let indexPath = viewController.indexPathForCallLog {
                    update(indexPath: indexPath, callProspectName:callProspectName, callDetails:callDetails, callType:callType, callTime: callTime)
                } else {
                    save(callProspectName:callProspectName, callDetails:callDetails, callType:callType, callTime: callTime)
                }
            }
            tableView.reloadData()
        }
        else if let viewController = segue.source as? CallLogDetailViewController {
            tableView.reloadData()
        }
    }
}

