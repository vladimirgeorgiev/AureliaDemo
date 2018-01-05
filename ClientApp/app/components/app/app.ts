import { Aurelia, PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';

export class App {
    router: Router;

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'aureliadotnetcore';
        config.map([{
            route: ['', 'home'],
            name: 'home',
            settings: { icon: 'home' },
            moduleId: PLATFORM.moduleName('../home/home'),
            nav: true,
            title: 'Home'
        }, {
            route: 'counter',
            name: 'counter',
            settings: { icon: 'education' },
            moduleId: PLATFORM.moduleName('../counter/counter'),
            nav: true,
            title: 'Counter'
        }, {
            route: 'fetch-data',
            name: 'fetchdata',
            settings: { icon: 'th-list' },
            moduleId: PLATFORM.moduleName('../fetchdata/fetchdata'),
            nav: true,
            title: 'Fetch data'
        }, {
            route: 'movies',
            name: 'movies',
            settings: { icon: 'usd' },
            moduleId: PLATFORM.moduleName('../movies/List'),
            nav: true,
            title: 'Movies'
        },
        {
            route: 'about',
            name: 'about',
            settings: { icon: 'signal' },
            moduleId: PLATFORM.moduleName('../about/about'),
            nav: true,
            title: 'About'
        },
        {
            route: 'details/:id',
            name: 'details',
            moduleId: PLATFORM.moduleName('../movies/details'),
            title: 'details',
            nav: false
        },
        { route: "edit/:id", moduleId: PLATFORM.moduleName('../movies/edit'), name: 'edit' },
        { route: "create", moduleId: PLATFORM.moduleName('../movies/edit'), name: 'create' },
        {
            route: 'aggrid',
            name: 'aggrid',
            settings: { icon: 'signal' },
            moduleId: PLATFORM.moduleName('../aggrid/list'),
            nav: true,
            title: 'Aggrid' }
        ]);

        this.router = router;
    }
}
