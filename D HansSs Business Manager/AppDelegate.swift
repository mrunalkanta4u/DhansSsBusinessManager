//
//  AppDelegate.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit
import CoreData

@UIApplicationMain
class AppDelegate: UIResponder, UIApplicationDelegate, UISplitViewControllerDelegate {

    var window: UIWindow?

    func application(_ application: UIApplication, didFinishLaunchingWithOptions launchOptions: [UIApplicationLaunchOptionsKey: Any]?) -> Bool {
        // Override point for customization after application launch.
        
        let tabBarViewController = self.window!.rootViewController as! UITabBarController
        print(tabBarViewController.viewControllers?.count as Any)
        
        var nameListSplitViewController:UISplitViewController? = nil
        var dreamListSplitViewController:UISplitViewController? = nil
        var teamListSplitViewController:UISplitViewController? = nil
        var callLogSplitViewController:UISplitViewController? = nil
        var miscellaneousTableViewController:UITableViewController? = nil
        
        for viewController in tabBarViewController.viewControllers! {
            if viewController.title == "NameList" {
                nameListSplitViewController = viewController as? UISplitViewController
                let nameListNavigationController = nameListSplitViewController!.viewControllers[nameListSplitViewController!.viewControllers.count-1] as! UINavigationController
                nameListNavigationController.topViewController!.navigationItem.leftBarButtonItem = nameListSplitViewController!.displayModeButtonItem
                nameListSplitViewController!.delegate = self
                let nameListMasterNavigationController = nameListSplitViewController?.viewControllers[0] as! UINavigationController
                let nameListController = nameListMasterNavigationController.topViewController as! NameListViewController
                nameListController.managedObjectContext = self.persistentContainer.viewContext
            }
            if viewController.title == "DreamList" {
                dreamListSplitViewController = viewController as? UISplitViewController
                let dreamListNavigationController = dreamListSplitViewController!.viewControllers[dreamListSplitViewController!.viewControllers.count-1] as! UINavigationController
                dreamListNavigationController.topViewController!.navigationItem.leftBarButtonItem = dreamListSplitViewController!.displayModeButtonItem
                dreamListSplitViewController!.delegate = self
                let dreamListMasterNavigationController = dreamListSplitViewController?.viewControllers[0] as! UINavigationController
                let dreamListController = dreamListMasterNavigationController.topViewController as! DreamListViewController
                dreamListController.managedObjectContext = self.persistentContainer.viewContext
            }
            if viewController.title == "TeamList" {
                teamListSplitViewController = viewController as? UISplitViewController
                let teamListNavigationController = teamListSplitViewController!.viewControllers[teamListSplitViewController!.viewControllers.count-1] as! UINavigationController
                teamListNavigationController.topViewController!.navigationItem.leftBarButtonItem = teamListSplitViewController!.displayModeButtonItem
                teamListSplitViewController!.delegate = self
                let teamListMasterNavigationController = teamListSplitViewController?.viewControllers[0] as! UINavigationController
                let teamListController = teamListMasterNavigationController.topViewController as! TeamListViewController
                teamListController.managedObjectContext = self.persistentContainer.viewContext
            }
            if viewController.title == "CallLog" {
                callLogSplitViewController = viewController as? UISplitViewController
                let callLogNavigationController = callLogSplitViewController!.viewControllers[callLogSplitViewController!.viewControllers.count-1] as! UINavigationController
                callLogNavigationController.topViewController!.navigationItem.leftBarButtonItem = callLogSplitViewController!.displayModeButtonItem
                callLogSplitViewController!.delegate = self
                let callLogMasterNavigationController = callLogSplitViewController?.viewControllers[0] as! UINavigationController
                let callLogController = callLogMasterNavigationController.topViewController as! CallLogViewController
                callLogController.managedObjectContext = self.persistentContainer.viewContext
            }
//            if viewController.title == "Miscellaneous" {
//                miscellaneousTableViewController = viewController as! MiscellaneousViewController
////                miscellaneousTableViewController.delegate = self
//                miscellaneousTableViewController.managedObjectContext = self.persistentContainer.viewContext
//            }
            
        }
        
        return true
    }

    func applicationWillResignActive(_ application: UIApplication) {
        // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
        // Use this method to pause ongoing tasks, disable timers, and invalidate graphics rendering callbacks. Games should use this method to pause the game.
    }

    func applicationDidEnterBackground(_ application: UIApplication) {
        // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later.
        // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
    }

    func applicationWillEnterForeground(_ application: UIApplication) {
        // Called as part of the transition from the background to the active state; here you can undo many of the changes made on entering the background.
    }

    func applicationDidBecomeActive(_ application: UIApplication) {
        // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
    }

    func applicationWillTerminate(_ application: UIApplication) {
        // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
        // Saves changes in the application's managed object context before the application terminates.
        self.saveContext()
    }

    // MARK: - Split view

    func splitViewController(_ splitViewController: UISplitViewController, collapseSecondary secondaryViewController:UIViewController, onto primaryViewController:UIViewController) -> Bool {
        let navigationTitle = secondaryViewController.title
        guard let secondaryAsNavController = secondaryViewController as? UINavigationController else { return false }
//        let mrunalVal = secondaryAsNavController.topViewController?.title
        if (splitViewController.title == "NameList" && navigationTitle == "contactDetailsNavigation" )
        {
            guard let topAsDreamDetailController = secondaryAsNavController.topViewController as? NameListDetailViewController else { return false }
            if topAsDreamDetailController.detailItem == nil {
                // Return true to indicate that we have handled the collapse by doing nothing; the secondary controller will be discarded.
                return true
            }
        }
        if (splitViewController.title == "DreamList" && navigationTitle == "dreamDetailsNavigation" )
        {
            guard let topAsDreamDetailController = secondaryAsNavController.topViewController as? DreamListDetailViewController else { return false }
            if topAsDreamDetailController.detailItem == nil {
                // Return true to indicate that we have handled the collapse by doing nothing; the secondary controller will be discarded.
                return true
            }
        }
        if (splitViewController.title == "TeamList" && navigationTitle == "teamDetailsNavigation" )
        {
            guard let topAsTeamDetailController = secondaryAsNavController.topViewController as? TeamListDetailViewController else { return false }
            if topAsTeamDetailController.detailItem == nil {
                // Return true to indicate that we have handled the collapse by doing nothing; the secondary controller will be discarded.
                return true
            }
        }
        if (splitViewController.title == "CallLog" && navigationTitle == "callLogDetailsNavigation" )
        {
           guard let topAsCallLogDetailController = secondaryAsNavController.topViewController as? CallLogDetailViewController else { return false }
            if topAsCallLogDetailController.detailItem == nil {
                // Return true to indicate that we have handled the collapse by doing nothing; the secondary controller will be discarded.
             return true
            }
        }
        return false
    }
    // MARK: - Core Data stack

    lazy var persistentContainer: NSPersistentContainer = {
        /*
         The persistent container for the application. This implementation
         creates and returns a container, having loaded the store for the
         application to it. This property is optional since there are legitimate
         error conditions that could cause the creation of the store to fail.
        */
        let container = NSPersistentContainer(name: "D_HansSs_Business_Manager")
        container.loadPersistentStores(completionHandler: { (storeDescription, error) in
            if let error = error as NSError? {
                // Replace this implementation with code to handle the error appropriately.
                // fatalError() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development.
                 
                /*
                 Typical reasons for an error here include:
                 * The parent directory does not exist, cannot be created, or disallows writing.
                 * The persistent store is not accessible, due to permissions or data protection when the device is locked.
                 * The device is out of space.
                 * The store could not be migrated to the current model version.
                 Check the error message to determine what the actual problem was.
                 */
                fatalError("Unresolved error \(error), \(error.userInfo)")
            }
        })
        return container
    }()

    // MARK: - Core Data Saving support

    func saveContext () {
        let context = persistentContainer.viewContext
        if context.hasChanges {
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

}
extension UIImageView {
    
    func setRounded() {
        self.layer.cornerRadius = (self.frame.width / 2) //instead of let radius = CGRectGetWidth(self.frame) / 2
        self.layer.masksToBounds = true
    }
}
