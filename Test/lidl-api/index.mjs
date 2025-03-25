import 'dotenv/config';
import { Issuer, generators } from 'openid-client';
import puppeteer from 'puppeteer';
import url from 'url';
import axios from 'axios';

const iPhone = puppeteer.devices['iPhone X'];


const login_email = process.env.LIDL_EMAIL;
const login_password = process.env.LIDL_PASSWORD;

const openidIssuer = await Issuer.discover('https://accounts.lidl.com');
const nonce = generators.nonce();
const code_verifier = generators.codeVerifier();
const code_challenge = generators.codeChallenge(code_verifier);

const client = new openidIssuer.Client({
    client_id: 'LidlPlusNativeClient',
    redirect_uris: ['com.lidlplus.app://callback'],
    response_types: ['code']
});

const loginurl = client.authorizationUrl({
    scope: 'openid profile offline_access lpprofile lpapis',
    code_challenge,
    code_challenge_method: 'S256'
});

const browser = await puppeteer.launch({ headless: false, slowMo: 50 });
const page = await browser.newPage();
await page.emulate(iPhone);
await page.setRequestInterception(true);

page.on('request', async (request) => {
    if (request.isNavigationRequest() && request.url().includes('com.lidlplus.app://callback')) {
        const url_parts = url.parse(request.url(), true);
        const query = url_parts.query;

        const tokenurl = 'https://accounts.lidl.com/connect/token';
        const headers = {
            'Authorization': 'Basic TGlkbFBsdXNOYXRpdmVDbGllbnQ6c2VjcmV0',
            'Content-Type': 'application/x-www-form-urlencoded'
        };

        const form = new URLSearchParams({
            grant_type: 'authorization_code',
            code: query.code,
            redirect_uri: 'com.lidlplus.app://callback',
            code_verifier: code_verifier
        });

        const response = await axios.post(tokenurl, form, { headers });
        console.log('\n✅ Access token:', response.data.access_token);
        console.log('\n✅ Refresh token:', response.data.refresh_token);
        await browser.close();
    }
    request.continue();
});

await page.goto(loginurl, { waitUntil: 'networkidle0' });

await page.click('#btn_continue_login');
await page.waitForSelector('#EmailPhone');
await page.type('#EmailPhone', login_email);
await page.click('#btn_submit_email');
await page.waitForTimeout(2000);

await page.type('#Password', login_password);
await page.click('#btn_submit_password');
