import { AppPage } from './app.po';

describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getMainHeading()).toEqual('Contact Manager App go here');
  });
});
