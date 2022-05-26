//
//  MasterViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit
import CoreData

class TeamListViewController: UITableViewController, NSFetchedResultsControllerDelegate, UISearchResultsUpdating {
    
    var detailViewController: TeamListDetailViewController? = nil
    var managedObjectContext: NSManagedObjectContext? = nil
    
    let searchController = UISearchController(searchResultsController: nil)
    var searchPredicate: NSPredicate? = nil
    var filteredObjects : [TeamMember]? = nil
    
    
    
//
//    let today = Date()
//    let calendar = NSCalendar.current
//    //let components = calendar.dateComponents([.day, .month, .year], from: today)
//
//    var dateComp:DateComponents = DateComponents()
//    dateComp.day = 20//components.day
//    dateComp.month = 10//components.month
//    dateComp.year = 2017//components.year
//    dateComp.hour = 20
//    dateComp.minute = 50
//    let date = calendar.date(from: dateComp)
//
//    let dateFormatter = DateFormatter()
//    dateFormatter.dateFormat = "dd MM yyyy hh:mm:ss"
//    let fireDate = dateFormatter.string(from: date!)
//    print("fireDate: \(fireDate)")
//
//
//    localNotificationSilent.fireDate = date
//    // no need to set time zone Remove bellow line
//    localNotificationSilent.timeZone = NSCalendar.currentCalendar().timeZone
//
//
//    let localNotificationSilent = UILocalNotification()
//    localNotificationSilent.fireDate = date
//    localNotificationSilent.repeatInterval = .day
//    localNotificationSilent.alertBody = "Started!"
//    localNotificationSilent.alertAction = "swipe to hear!"
//    localNotificationSilent.category = "PLAY_CATEGORY"
//    UIApplication.shared.scheduleLocalNotification(localNotificationSilent)
//
//
//
//
    
    
    
    func filterContentForSearchText(searchText: String, scope: String = "All" )
    {
        searchPredicate = NSPredicate(format: "teamMemberName contains[c] %@", searchText)
        filteredObjects = self.fetchedResultsController.fetchedObjects?.filter() {
            return self.searchPredicate!.evaluate(with: $0)
            } as [TeamMember]?
        self.tableView.reloadData()
    }
    func updateSearchResults(for searchController: UISearchController) {
        filterContentForSearchText(searchText: searchController.searchBar.text!)
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        navigationItem.leftBarButtonItem = editButtonItem
        
        if let split = splitViewController {
            let controllers = split.viewControllers
            detailViewController = (controllers[controllers.count-1] as! UINavigationController).topViewController as? TeamListDetailViewController
        }
        
        // UISearchController setup
        searchController.dimsBackgroundDuringPresentation = false
        searchController.searchResultsUpdater = self
        self.tableView.tableHeaderView = searchController.searchBar
        self.definesPresentationContext = true
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
        if segue.identifier == "showTeamMemberDetail" {
            if let indexPath = tableView.indexPathForSelectedRow {
                var object : TeamMember? = nil
                if searchController.isActive && searchController.searchBar.text != ""
                {
                    object = filteredObjects?[indexPath.row]
                }
                else{
                    object = fetchedResultsController.object(at: indexPath)
                }
                let controller = (segue.destination as! UINavigationController).topViewController as! TeamListDetailViewController
                controller.detailItem = object
                controller.navigationItem.leftBarButtonItem = splitViewController?.displayModeButtonItem
                controller.navigationItem.leftItemsSupplementBackButton = true
                controller.indexPath = indexPath
            }
        }
    }
    
    
    // MARK: - Table View
    
    override func numberOfSections(in tableView: UITableView) -> Int {
        return fetchedResultsController.sections?.count ?? 0
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if searchController.isActive && searchController.searchBar.text != ""{
            return (filteredObjects?.count)!
        }
        let sectionInfo = fetchedResultsController.sections![section]
        return sectionInfo.numberOfObjects
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "TeamMemberCell", for: indexPath)
        
        var teamMember : TeamMember? = nil
        if searchController.isActive && searchController.searchBar.text != ""{
            teamMember = filteredObjects?[indexPath.row]
        }else{
            teamMember = fetchedResultsController.object(at: indexPath)
        }
        configureCell(cell, withTeamMember: teamMember!)
        return cell
    }
    
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
    
    func configureCell(_ cell: UITableViewCell, withTeamMember teamMember: TeamMember) {
        
        cell.textLabel!.text = teamMember.teamMemberName!.description
        cell.detailTextLabel?.text = teamMember.teamMemberID!.description
//        cell.imageView?.image = UIImage(data: teamMember.tea!)
        //TODO - save image saving of default type again and again
    }
    
    // MARK: - Fetched results controller
    
    var fetchedResultsController: NSFetchedResultsController<TeamMember> {
        if _fetchedResultsController != nil {
            return _fetchedResultsController!
        }

        let fetchRequest: NSFetchRequest<TeamMember> = TeamMember.fetchRequest()

        // Set the batch size to a suitable number.
        fetchRequest.fetchBatchSize = 20

        // Edit the sort key as appropriate.
        let sortDescriptor = NSSortDescriptor(key: "teamMemberName", ascending: false)

        fetchRequest.sortDescriptors = [sortDescriptor]

        // Edit the section name key path and cache name if appropriate.
        // nil for section name key path means "no sections".
        let aFetchedResultsController = NSFetchedResultsController(fetchRequest: fetchRequest, managedObjectContext: self.managedObjectContext!, sectionNameKeyPath: nil, cacheName: "TeamList")

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
    var _fetchedResultsController: NSFetchedResultsController<TeamMember>? = nil
    
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
            configureCell(tableView.cellForRow(at: indexPath!)!, withTeamMember: anObject as! TeamMember)
        case .move:
            configureCell(tableView.cellForRow(at: indexPath!)!, withTeamMember: anObject as! TeamMember)
            tableView.moveRow(at: indexPath!, to: newIndexPath!)
        }
    }
    
    func controllerDidChangeContent(_ controller: NSFetchedResultsController<NSFetchRequestResult>) {
        tableView.endUpdates()
    }
    

    func save(teamMemberName: String, teamMemberID: String, teamMemberIsActive: Bool, teamMemberIsDR: Bool, teamMemberKYC: Bool, teamMemberPassword1: String, teamMemberPassword2: String, teamMemberSecurityInfo: String, teamMemberTicketSize: Int) {
        let context = self.fetchedResultsController.managedObjectContext
        let newTeamMember = TeamMember(context: context)
        // If appropriate, configure the new managed object.
        newTeamMember.teamMemberName = teamMemberName
        newTeamMember.teamMemberID = teamMemberID
        newTeamMember.teamMemberIsActive = teamMemberIsActive
        newTeamMember.teamMemberIsDR = teamMemberIsDR
        newTeamMember.teamMemberKYC = false
        newTeamMember.teamMemberPassword1 = teamMemberPassword1
        newTeamMember.teamMemberPassword2 = teamMemberPassword2
        newTeamMember.teamMemberSecurityInfo = teamMemberSecurityInfo
        newTeamMember.teamMemberTicketSize = Int32(teamMemberTicketSize)

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
    
    
    func update(indexPath: IndexPath, teamMemberName: String, teamMemberID: String, teamMemberIsActive: Bool, teamMemberIsDR: Bool, teamMemberKYC: Bool, teamMemberPassword1: String, teamMemberPassword2: String, teamMemberSecurityInfo: String, teamMemberTicketSize: Int) {
        
        let context = self.fetchedResultsController.managedObjectContext
        fetchedResultsController.object(at: indexPath).teamMemberName = teamMemberName
        fetchedResultsController.object(at: indexPath).teamMemberID = teamMemberID
        fetchedResultsController.object(at: indexPath).teamMemberIsActive = teamMemberIsActive
        fetchedResultsController.object(at: indexPath).teamMemberIsDR = teamMemberIsDR
        fetchedResultsController.object(at: indexPath).teamMemberKYC = teamMemberKYC
        fetchedResultsController.object(at: indexPath).teamMemberPassword1 = teamMemberPassword1
        fetchedResultsController.object(at: indexPath).teamMemberPassword2 = teamMemberPassword2
        fetchedResultsController.object(at: indexPath).teamMemberSecurityInfo = teamMemberSecurityInfo
        fetchedResultsController.object(at: indexPath).teamMemberTicketSize = Int32(teamMemberTicketSize)
        do {

            try context.save()
        } catch {
            // Replace this implementation with code to handle the error appropriately.
            // fatalError() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development.
            let nserror = error as NSError
            fatalError("Unresolved error \(nserror), \(nserror.userInfo)")
        }
    }


    @IBAction func unwindToTeamList(segue: UIStoryboardSegue) {
        if let viewController = segue.source as? AddTeamViewController {
            guard let teamMemberName: String = viewController.teamMemberNameTextField.text,
                let teamMemberID: String = viewController.teamMemberIDTextField.text,
                let teamMemberTicketSize: Int = Int(viewController.teamMemberTicketSize.text!),
                let teamMemberPassword1TextField: String = viewController.teamMemberPassword1TextField.text,
                let teamMemberPassword2TextField: String = viewController.teamMemberPassword2TextField.text,
                let teamMemberSequrityQATextField: String = viewController.teamMemberSequrityQATextField.text,
                let teamMemberIsDR: Bool? = viewController.teamMemberIsDRSwitch.isOn,
                let teamMemberIsActive: Bool? = viewController.teamMemberIsActiveSwitch.isOn,
                let teamMemberIsKYC: Bool? = false
                else { return }
            
            if teamMemberName != "" && teamMemberID != "" {
                if let indexPath = viewController.indexPathForTeam {
                    update(indexPath: indexPath, teamMemberName:teamMemberName, teamMemberID:teamMemberID, teamMemberIsActive:teamMemberIsActive!, teamMemberIsDR:teamMemberIsDR!, teamMemberKYC: teamMemberIsKYC!, teamMemberPassword1:teamMemberPassword1TextField, teamMemberPassword2:teamMemberPassword2TextField, teamMemberSecurityInfo:teamMemberSequrityQATextField, teamMemberTicketSize:teamMemberTicketSize)
                } else {
                    save(teamMemberName:teamMemberName, teamMemberID:teamMemberID, teamMemberIsActive:teamMemberIsActive!, teamMemberIsDR:teamMemberIsDR!, teamMemberKYC: teamMemberIsKYC!, teamMemberPassword1:teamMemberPassword1TextField, teamMemberPassword2:teamMemberPassword2TextField, teamMemberSecurityInfo:teamMemberSequrityQATextField, teamMemberTicketSize:teamMemberTicketSize)
                }
            }
            tableView.reloadData()
        }
        else if let viewController = segue.source as? TeamListDetailViewController {
            tableView.reloadData()
        }
    }
}

