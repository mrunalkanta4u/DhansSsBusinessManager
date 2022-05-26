//
//  OnboardingPageViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 31/10/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class OnboardingPageViewController: UIViewController, UIPageViewControllerDataSource
{
    var pageViewController : UIPageViewController?
    var pageTitles = ["God vs Man", "Cool Breeze", "Fire Sky"]
    var pageImages = ["NameList", "DreamList", "TeamList"]
    var currentIndex : Int = 0
    
    override func viewDidLoad()
    {
        super.viewDidLoad()
        
        pageViewController = UIPageViewController(transitionStyle: .scroll, navigationOrientation: .horizontal, options: nil)
        pageViewController!.dataSource = self
        
        let startingViewController: WelcomeViewController = viewControllerAtIndex(index: 0)!
        let viewControllers = [startingViewController]
        pageViewController!.setViewControllers(viewControllers , direction: .forward, animated: false, completion: nil)
        pageViewController!.view.frame = CGRect(x: 0, y: 0, width: view.frame.size.width, height: view.frame.size.height);
        
        addChildViewController(pageViewController!)
        view.addSubview(pageViewController!.view)
        pageViewController!.didMove(toParentViewController: self)
    }
    
    override func didReceiveMemoryWarning()
    {
        super.didReceiveMemoryWarning()
    }
    
    func pageViewController(_ pageViewController: UIPageViewController, viewControllerBefore viewController: UIViewController) -> UIViewController?
    {
        var index = (viewController as! WelcomeViewController).pageIndex
        
        if (index == 0) || (index == NSNotFound) {
            return nil
        }
        
        index -= 1
        
        return viewControllerAtIndex(index: index)
    }
    
    func pageViewController(_ pageViewController: UIPageViewController, viewControllerAfter viewController: UIViewController) -> UIViewController?
    {
        var index = (viewController as! WelcomeViewController).pageIndex
        
        if index == NSNotFound {
            return nil
        }
        
        index += 1
        
        if (index == self.pageTitles.count) {
            return nil
        }
        
        return viewControllerAtIndex(index: index)
    }
    
    func viewControllerAtIndex(index: Int) -> WelcomeViewController?
    {
        if self.pageTitles.count == 0 || index >= self.pageTitles.count
        {
            return nil
        }
        
        // Create a new view controller and pass suitable data.
        let pageContentViewController = WelcomeViewController()
//        pageContentViewController.welcomeImageView.image = #imageLiteral(resourceName: "NameList") // UIImage(named: pageImages[index])
//        pageContentViewController.welcomeTitleTextField.text = "mkm"// pageTitles[index]
        pageContentViewController.pageIndex = index
        currentIndex = index
        
        return pageContentViewController
    }
    
    func presentationCountForPageViewController(pageViewController: UIPageViewController) -> Int
    {
        return self.pageTitles.count
    }
    
    func presentationIndexForPageViewController(pageViewController: UIPageViewController) -> Int
    {
        return 0
    }
    
}
//
//class OnboardingPageViewController: UIPageViewController
//    {
//        var arrPageTitle: NSArray = NSArray()
//        var arrPagePhoto: NSArray = NSArray()
//
//        override func viewDidLoad() {
//            super.viewDidLoad()
//
//            arrPageTitle = ["Name List", "Dream List", "Team List"];
//            arrPagePhoto = ["NameList", "DreamList", "TeamList"];
//
//            self.dataSource = self as? UIPageViewControllerDataSource
//
//            self.setViewControllers([getViewControllerAtIndex(index: 0)] as [UIViewController], direction: UIPageViewControllerNavigationDirection.forward, animated: false, completion: nil)
//        }
//
//        // MARK:- UIPageViewControllerDataSource Methods
//
//        func pageViewController(pageViewController: UIPageViewController, viewControllerBeforeViewController viewController: UIViewController) -> UIViewController?
//        {
//            let pageContent: WelcomeViewController = viewController as! WelcomeViewController
//
//            var index: Int = pageContent.pageIndex
//
//            if ((index == 0) || (index == NSNotFound))
//            {
//                return nil
//            }
//
//            index -= 1
//            return getViewControllerAtIndex(index: index)
//        }
//
//        func pageViewController(pageViewController: UIPageViewController, viewControllerAfterViewController viewController: UIViewController) -> UIViewController?
//        {
//            let pageContent: WelcomeViewController = viewController as! WelcomeViewController
//
//            var index: Int = pageContent.pageIndex
//
//            if (index == NSNotFound)
//            {
//                return nil;
//            }
//
//            index += 1
//            if (index == arrPageTitle.count)
//            {
//                return nil;
//            }
//            return getViewControllerAtIndex(index: index)
//        }
//
//        // MARK:- Other Methods
//        func getViewControllerAtIndex(index: NSInteger) -> WelcomeViewController
//        {
//            // Create a new view controller and pass suitable data.
//            let pageContentViewController = self.storyboard?.instantiateViewController(withIdentifier: "WelcomeViewController") as! WelcomeViewController
//
//            pageContentViewController.strTitle = "\(arrPageTitle[index])"
//            pageContentViewController.strPhotoName = "\(arrPagePhoto[index])"
//            pageContentViewController.pageIndex = index
//
//            return pageContentViewController
//        }
//}

